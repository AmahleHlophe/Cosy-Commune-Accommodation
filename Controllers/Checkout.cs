using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Tenant, Manager")]
public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;

    // 1. Constructor for Dependency Injection (DI)
    public CheckoutController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- READ Operations ---

    // GET: Rooms
    // Displays a list of all rooms.
    public async Task<IActionResult> Index()
    {
        return View(await _context.Checkouts.ToListAsync());
    }

    // GET: Rooms/Details/5
    // Displays details for a single room.
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var checkout = await _context.Checkouts
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (checkout == null)
        {
            return NotFound();
        }

        return View(checkout);
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
    public async Task<IActionResult> Create([Bind("UserId,VacateDate,Reason,Status,CreatedAt")] Checkout checkout)
    {
        if (ModelState.IsValid)
        {
            _context.Add(checkout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(checkout);
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

        var checkout = await _context.Checkouts.FindAsync(id);
        if (checkout == null)
        {
            return NotFound();
        }
        return View(checkout);
    }

    // POST: Rooms/Edit/5
    // Handles the form submission and updates the existing room in the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserId,VacateDate,Reason,Status,CreatedAt")] Checkout checkout)
    {
        if (id != checkout.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(checkout); // Marks the entity as modified
                await _context.SaveChangesAsync(); // Executes the UPDATE command
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Checkouts.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(checkout);
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

        var checkout = await _context.Checkouts
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (checkout == null)
        {
            return NotFound();
        }

        return View(checkout);
    }

    // POST: Rooms/Delete/5
    // Executes the deletion of the room from the database.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var checkout = await _context.Checkouts.FindAsync(id);
        if (checkout != null)
        {
            _context.Checkouts.Remove(checkout); // Marks the entity for deletion
            await _context.SaveChangesAsync(); // Executes the DELETE command
        }
        return RedirectToAction(nameof(Index));
    }
}