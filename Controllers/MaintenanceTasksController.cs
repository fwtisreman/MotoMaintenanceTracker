
using Microsoft.AspNetCore.Mvc;
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

    // GET: MAINTENANCETASKS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.MaintenanceTasks.ToListAsync());
    }

    // GET: MAINTENANCETASKS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var maintenancetask = await _context.MaintenanceTasks
            .FirstOrDefaultAsync(m => m.Id == id);
        if (maintenancetask == null)
        {
            return NotFound();
        }

        return View(maintenancetask);
    }

    // GET: MAINTENANCETASKS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MAINTENANCETASKS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,DueMileage,IsCompleted")] MaintenanceTask maintenancetask)
    {
        if (ModelState.IsValid)
        {
            _context.Add(maintenancetask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(maintenancetask);
    }

    // GET: MAINTENANCETASKS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var maintenancetask = await _context.MaintenanceTasks.FindAsync(id);
        if (maintenancetask == null)
        {
            return NotFound();
        }
        return View(maintenancetask);
    }

    // POST: MAINTENANCETASKS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,DueDate,DueMileage,IsCompleted")] MaintenanceTask maintenancetask)
    {
        if (id != maintenancetask.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(maintenancetask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceTaskExists(maintenancetask.Id))
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
        return View(maintenancetask);
    }

    // GET: MAINTENANCETASKS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var maintenancetask = await _context.MaintenanceTasks
            .FirstOrDefaultAsync(m => m.Id == id);
        if (maintenancetask == null)
        {
            return NotFound();
        }

        return View(maintenancetask);
    }

    // POST: MAINTENANCETASKS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var maintenancetask = await _context.MaintenanceTasks.FindAsync(id);
        if (maintenancetask != null)
        {
            _context.MaintenanceTasks.Remove(maintenancetask);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MaintenanceTaskExists(int? id)
    {
        return _context.MaintenanceTasks.Any(e => e.Id == id);
    }
}
