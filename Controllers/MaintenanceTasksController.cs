using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Data;
using MotoMaintenanceTracker.Models;

namespace MotoMaintenanceTracker.Controllers
{
    public class MaintenanceTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Shows all tasks, except those of deleted motorcycles
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.MaintenanceTasks
                .Include(t => t.Motorcycle)
                .Where(t => t.Motorcycle == null || !t.Motorcycle.IsDeleted)
                .ToListAsync();

            return View(tasks);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.MaintenanceTasks
                .Include(t => t.Motorcycle)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            // The view shows the reminder message for this task, if there is one
            ViewData["MaintenanceReminder"] = await _context.Messages
                .FirstOrDefaultAsync(m => m.MaintenanceTaskId == task.Id);

            return View(task);
        }

        public async Task<IActionResult> Create()
        {
            await LoadMotorcyclesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MotorcycleId,Title,Description,DueDate,DueMileage,IsCompleted")] MaintenanceTask task)
        {
            await ValidateTaskAsync(task);

            if (!ModelState.IsValid)
            {
                await LoadMotorcyclesAsync(task.MotorcycleId);
                return View(task);
            }

            _context.MaintenanceTasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.MaintenanceTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await LoadMotorcyclesAsync(task.MotorcycleId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,MotorcycleId,Title,Description,DueDate,DueMileage,IsCompleted")] MaintenanceTask task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            await ValidateTaskAsync(task);

            if (!ModelState.IsValid)
            {
                await LoadMotorcyclesAsync(task.MotorcycleId);
                return View(task);
            }

            var existing = await _context.MaintenanceTasks.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // If the due date changes, the reminders must be able to fire again
            if (existing.DueDate != task.DueDate)
            {
                existing.SevenDayReminderSent = false;
                existing.OneDayReminderSent = false;
                existing.DueDateReminderSent = false;
            }

            // Copy only the fields the user can edit
            existing.MotorcycleId = task.MotorcycleId;
            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.DueDate = task.DueDate;
            existing.DueMileage = task.DueMileage;
            existing.IsCompleted = task.IsCompleted;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.MaintenanceTasks
                .Include(t => t.Motorcycle)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.MaintenanceTasks.FindAsync(id);

            if (task != null)
            {
                _context.MaintenanceTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Fills the motorcycle dropdown, and the mileages the form uses
        private async Task LoadMotorcyclesAsync(int? selectedId = null)
        {
            var motorcycles = await _context.Motorcycles
                .Where(m => !m.IsDeleted)
                .Select(m => new
                {
                    m.Id,
                    DisplayName = m.Brand + " " + m.Model,
                    m.Mileage
                })
                .ToListAsync();

            ViewData["MotorcycleId"] = new SelectList(motorcycles, "Id", "DisplayName", selectedId);
            ViewData["MotorcycleMileages"] = motorcycles.ToDictionary(m => m.Id, m => m.Mileage);
        }

        // Checks the rules that data annotations can't express
        private async Task ValidateTaskAsync(MaintenanceTask task)
        {
            if (task.DueDate.HasValue && task.DueDate.Value.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(task.DueDate),
                    "Due date cannot be in the past.");
            }

            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == task.MotorcycleId && !m.IsDeleted);

            if (motorcycle == null)
            {
                ModelState.AddModelError(nameof(task.MotorcycleId),
                    "Please select an active motorcycle.");
            }
            else if (task.DueMileage.HasValue && task.DueMileage.Value < motorcycle.Mileage)
            {
                ModelState.AddModelError(nameof(task.DueMileage),
                    $"Due mileage cannot be lower than the current mileage ({motorcycle.Mileage:N0} km).");
            }
        }
    }
}