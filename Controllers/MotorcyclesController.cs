
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Models;
using MotoMaintenanceTracker.Data;

public class MotorcyclesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MotorcyclesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MOTORCYCLES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Motorcycles.ToListAsync());
    }

    // GET: MOTORCYCLES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var motorcycle = await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (motorcycle == null)
        {
            return NotFound();
        }

        return View(motorcycle);
    }

    // GET: MOTORCYCLES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MOTORCYCLES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Brand,Model,Year,Mileage")] Motorcycle motorcycle)
    {
        if (ModelState.IsValid)
        {
            _context.Add(motorcycle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(motorcycle);
    }

    // GET: MOTORCYCLES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var motorcycle = await _context.Motorcycles.FindAsync(id);
        if (motorcycle == null)
        {
            return NotFound();
        }
        return View(motorcycle);
    }

    // POST: MOTORCYCLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Brand,Model,Year,Mileage")] Motorcycle motorcycle)
    {
        if (id != motorcycle.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(motorcycle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotorcycleExists(motorcycle.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(motorcycle);
    }

    // GET: MOTORCYCLES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var motorcycle = await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (motorcycle == null)
        {
            return NotFound();
        }

        return View(motorcycle);
    }

    // POST: MOTORCYCLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var motorcycle = await _context.Motorcycles.FindAsync(id);
        if (motorcycle != null)
        {
            _context.Motorcycles.Remove(motorcycle);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MotorcycleExists(int? id)
    {
        return _context.Motorcycles.Any(e => e.Id == id);
    }
}
