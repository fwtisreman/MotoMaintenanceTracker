using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Data;
using MotoMaintenanceTracker.Models;

namespace MotoMaintenanceTracker.Controllers
{
    public class ServiceRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _context.ServiceRecords
                .Include(s => s.Motorcycle)
                .ToListAsync();

            return View(records);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.ServiceRecords
                .Include(s => s.Motorcycle)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        public async Task<IActionResult> Create()
        {
            await LoadMotorcyclesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MotorcycleId,ServiceType,ServiceDate,Mileage,Cost,Notes")] ServiceRecord serviceRecord)
        {
            await ValidateRecordAsync(serviceRecord);

            if (!ModelState.IsValid)
            {
                await LoadMotorcyclesAsync(serviceRecord.MotorcycleId);
                return View(serviceRecord);
            }

            _context.ServiceRecords.Add(serviceRecord);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.ServiceRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            await LoadMotorcyclesAsync(record.MotorcycleId);
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,MotorcycleId,ServiceType,ServiceDate,Mileage,Cost,Notes")] ServiceRecord serviceRecord)
        {
            if (id != serviceRecord.Id)
            {
                return NotFound();
            }

            await ValidateRecordAsync(serviceRecord);

            if (!ModelState.IsValid)
            {
                await LoadMotorcyclesAsync(serviceRecord.MotorcycleId);
                return View(serviceRecord);
            }

            var existing = await _context.ServiceRecords.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Copy the edited values onto the record loaded from the database
            existing.MotorcycleId = serviceRecord.MotorcycleId;
            existing.ServiceType = serviceRecord.ServiceType;
            existing.ServiceDate = serviceRecord.ServiceDate;
            existing.Mileage = serviceRecord.Mileage;
            existing.Cost = serviceRecord.Cost;
            existing.Notes = serviceRecord.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.ServiceRecords
                .Include(s => s.Motorcycle)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _context.ServiceRecords.FindAsync(id);

            if (record != null)
            {
                _context.ServiceRecords.Remove(record);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Fills the motorcycle dropdown with motorcycles that are not deleted
        private async Task LoadMotorcyclesAsync(int? selectedId = null)
        {
            var motorcycles = await _context.Motorcycles
                .Where(m => !m.IsDeleted)
                .Select(m => new { m.Id, Name = m.Brand + " " + m.Model })
                .ToListAsync();

            ViewData["MotorcycleId"] = new SelectList(motorcycles, "Id", "Name", selectedId);
        }

        // Checks the rules that data annotations can't express
        private async Task ValidateRecordAsync(ServiceRecord record)
        {
            if (record.ServiceDate.Date > DateTime.Today)
            {
                ModelState.AddModelError(nameof(record.ServiceDate),
                    "Service date cannot be in the future.");
            }

            var motorcycleExists = await _context.Motorcycles
                .AnyAsync(m => m.Id == record.MotorcycleId && !m.IsDeleted);

            if (!motorcycleExists)
            {
                ModelState.AddModelError(nameof(record.MotorcycleId),
                    "Please select a valid motorcycle.");
            }
        }
    }
}