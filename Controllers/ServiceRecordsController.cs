
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Data;
using MotoMaintenanceTracker.Models;

public class ServiceRecordsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServiceRecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SERVICERECORDS
    public async Task<IActionResult> Index()
    {
        var serviceRecords = await _context.ServiceRecords
            .Include(s => s.Motorcycle)
            .ToListAsync();

        return View(serviceRecords);
    }
    // GET: SERVICERECORDS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerecord = await _context.ServiceRecords
            .Include(s => s.Motorcycle)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (servicerecord == null)
        {
            return NotFound();
        }

        return View(servicerecord);
    }

    // GET: SERVICERECORDS/Create
    public IActionResult Create()
    {
        ViewData["MotorcycleId"] = new SelectList(
        _context.Motorcycles,
        "Id",
        "Model");

        return View();
    }

    //// POST: SERVICERECORDS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,MotorcycleId,ServiceType,ServiceDate,Mileage,Cost,Notes")]
        ServiceRecord servicerecord)
    {
        if (ModelState.IsValid)
        {
            _context.Add(servicerecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["MotorcycleId"] = new SelectList(
            _context.Motorcycles,
            "Id",
            "Model",
            servicerecord.MotorcycleId);

        return View(servicerecord);
    }

    // GET: SERVICERECORDS/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerecord = await _context.ServiceRecords.FindAsync(id);

        if (servicerecord == null)
        {
            return NotFound();
        }

        ViewData["MotorcycleId"] = new SelectList(
            _context.Motorcycles,
            "Id",
            "Model",
            servicerecord.MotorcycleId);

        return View(servicerecord);
    }


    // POST: SERVICERECORDS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MotorcycleId,Motorcycle,ServiceType,ServiceDate,Mileage,Cost,Notes")] ServiceRecord servicerecord)
    {
        if (id != servicerecord.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicerecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRecordExists(servicerecord.Id))
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
        return View(servicerecord);
    }

    // GET: SERVICERECORDS/Delete/5

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerecord = await _context.ServiceRecords
            .Include(s => s.Motorcycle)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (servicerecord == null)
        {
            return NotFound();
        }

        return View(servicerecord);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var servicerecord = await _context.ServiceRecords
            .FindAsync(id);

        if (servicerecord != null)
        {
            _context.ServiceRecords.Remove(servicerecord);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


    private bool ServiceRecordExists(int? id)
    {
        return _context.ServiceRecords.Any(e => e.Id == id);
    }
}
