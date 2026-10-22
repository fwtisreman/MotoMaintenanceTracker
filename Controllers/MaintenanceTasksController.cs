
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Models;
using MotoMaintenanceTracker.Data;

public class MaintenanceTasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public MaintenanceTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _context.MaintenanceTasks
            .Include(t => t.Motorcycle)
            .ToListAsync();

        return View(tasks);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var task = await _context.MaintenanceTasks
            .Include(t => t.Motorcycle)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return NotFound();


        var reminder = await _context.Messages
            .FirstOrDefaultAsync(m => m.MaintenanceTaskId == task.Id);

        ViewData["MaintenanceReminder"] = reminder;


        return View(task);
    }

    public IActionResult Create()
    {
        LoadMotorcycles();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,MotorcycleId,Title,Description,DueDate,DueMileage,IsCompleted")]
        MaintenanceTask maintenanceTask)
    {
        await ValidateScheduleAsync(maintenanceTask);
        if (ModelState.IsValid)
        {
            var motorcycleExists = await _context.Motorcycles
                .AnyAsync(m => m.Id == maintenanceTask.MotorcycleId
                            && !m.IsDeleted);

            if (motorcycleExists)
            {
                _context.MaintenanceTasks.Add(maintenanceTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                nameof(maintenanceTask.MotorcycleId),
                "Please select an active motorcycle.");
        }

        LoadMotorcycles(maintenanceTask.MotorcycleId);
        return View(maintenanceTask);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var task = await _context.MaintenanceTasks.FindAsync(id);

        if (task == null)
            return NotFound();

        LoadMotorcycles(task.MotorcycleId);
        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,MotorcycleId,Title,Description,DueDate,DueMileage,IsCompleted")]
        MaintenanceTask maintenanceTask)
    {
        if (id != maintenanceTask.Id)
            return NotFound();
        await ValidateScheduleAsync(maintenanceTask);
        if (ModelState.IsValid)
        {
            var motorcycleExists = await _context.Motorcycles
                .AnyAsync(m => m.Id == maintenanceTask.MotorcycleId
                            && !m.IsDeleted);

            if (motorcycleExists)
            {
                try
                {
                    _context.Update(maintenanceTask);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceTaskExists(maintenanceTask.Id))
                        return NotFound();

                    throw;
                }
            }

            ModelState.AddModelError(
                nameof(maintenanceTask.MotorcycleId),
                "Please select an active motorcycle.");
        }

        LoadMotorcycles(maintenanceTask.MotorcycleId);
        return View(maintenanceTask);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var task = await _context.MaintenanceTasks
            .Include(t => t.Motorcycle)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return NotFound();

        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var task = await _context.MaintenanceTasks.FindAsync(id);

        if (task != null)
        {
            _context.MaintenanceTasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void LoadMotorcycles(int? selectedId = null)
    {
        var motorcycles = _context.Motorcycles
            .Where(m => !m.IsDeleted)
            .Select(m => new
            {
                m.Id,
                DisplayName = m.Brand + " " + m.Model,
                m.Mileage
            })
            .ToList();

        ViewData["MotorcycleId"] = new SelectList(
            motorcycles,
            "Id",
            "DisplayName",
            selectedId);

        ViewData["MotorcycleMileages"] = motorcycles.ToDictionary(
            m => m.Id,
            m => m.Mileage);
    }

    private bool MaintenanceTaskExists(int id)
    {
        return _context.MaintenanceTasks.Any(t => t.Id == id);

    }

    private async Task ValidateScheduleAsync(MaintenanceTask task)
    {
        if (task.DueDate.HasValue &&
            task.DueDate.Value.Date < DateTime.Today)
        {
            ModelState.AddModelError(
                nameof(task.DueDate),
                "Due date cannot be in the past.");
        }

        if (task.DueMileage.HasValue)
        {
            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m =>
                    m.Id == task.MotorcycleId &&
                    !m.IsDeleted);

            if (motorcycle != null &&
                task.DueMileage.Value < motorcycle.Mileage)
            {
                ModelState.AddModelError(
                    nameof(task.DueMileage),
                    $"Due mileage cannot be lower than the current mileage ({motorcycle.Mileage:N0} km).");
            }
        }
    }
}
