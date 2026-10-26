using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Data;
using MotoMaintenanceTracker.Models;

namespace MotoMaintenanceTracker.Controllers
{
    public class MotorcyclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotorcyclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Shows all motorcycles that are not deleted
        public async Task<IActionResult> Index()
        {
            var motorcycles = await _context.Motorcycles
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            return View(motorcycles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await GetActiveMotorcycle(id.Value);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Year,Mileage")] Motorcycle motorcycle)
        {
            if (!ModelState.IsValid)
            {
                return View(motorcycle);
            }

            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await GetActiveMotorcycle(id.Value);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Year,Mileage")] Motorcycle motorcycle)
        {
            if (id != motorcycle.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(motorcycle);
            }

            var existing = await GetActiveMotorcycle(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Copy only the fields the user can change
            existing.Brand = motorcycle.Brand;
            existing.Model = motorcycle.Model;
            existing.Year = motorcycle.Year;
            existing.Mileage = motorcycle.Mileage;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await GetActiveMotorcycle(id.Value);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        // Soft delete: the row stays in the database but is hidden
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motorcycle = await GetActiveMotorcycle(id);
            if (motorcycle == null)
            {
                return NotFound();
            }

            motorcycle.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Finds a motorcycle by id, but only if it is not deleted
        private async Task<Motorcycle?> GetActiveMotorcycle(int id)
        {
            return await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }
    }
}