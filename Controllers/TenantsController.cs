using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Tenant, Manager")]
public class TenantsController : Controller
{
    private readonly ApplicationDbContext _context;

    // 1. Constructor for Dependency Injection (DI)
    public TenantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    //MAIN
    public IActionResult Dashboard()
    {
        return View();
    }

    // --- READ Operations ---

    // GET: Rooms
    // Displays a list of all rooms.
    public async Task<IActionResult> Index()
    {
        return View(await _context.Tenants.ToListAsync());
    }

    // GET: Rooms/Details/5
    // Displays details for a single room.
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (tenant == null)
        {
            return NotFound();
        }

        return View(tenant);
    }

    // --- CREATE Operations ---

    // GET: Rooms/Create
    // Displays the empty form to create a new room.
    public IActionResult Create()
    {
        return View();
    }

    // POST: Rooms/Create
    // Handles the form submission and saves the new room to the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FullName,IdNumber,Email,CreatedAt")] Tenant tenant)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tenant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tenant);
    }

    // --- UPDATE Operations ---

    // GET: Rooms/Edit/5
    // Displays the form populated with the existing room data.
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        return View(tenant);
    }

    // POST: Rooms/Edit/5
    // Handles the form submission and updates the existing room in the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("FullName,IdNumber,Email,CreatedAt")] Tenant tenant)
    {
        if (id != tenant.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tenant); // Marks the entity as modified
                await _context.SaveChangesAsync(); // Executes the UPDATE command
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tenants.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tenant);
    }

    // --- DELETE Operations ---

    // GET: Rooms/Delete/5
    // Displays a confirmation page before deletion.
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (tenant == null)
        {
            return NotFound();
        }

        return View(tenant);
    }

    // POST: Rooms/Delete/5
    // Executes the deletion of the room from the database.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant != null)
        {
            _context.Tenants.Remove(tenant); // Marks the entity for deletion
            await _context.SaveChangesAsync(); // Executes the DELETE command
        }
        return RedirectToAction(nameof(Index));
    }
}