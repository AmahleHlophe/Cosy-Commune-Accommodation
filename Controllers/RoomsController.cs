using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles="Manager, Admin")]
public class RoomsController : Controller
{
    private readonly ApplicationDbContext _context;

    // 1. Constructor for Dependency Injection (DI)
    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // --- READ Operations ---

    // GET: Rooms
    // Displays a list of all rooms.
    public async Task<IActionResult> Index()
    {
        return View(await _context.Rooms.ToListAsync());
    }

    // GET: Rooms/Details/5
    // Displays details for a single room.
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
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
    public async Task<IActionResult> Create([Bind("RoomNumber,RoomType,Price,Capacity,IsOccupied")] Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(room);
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

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return View(room);
    }

    // POST: Rooms/Edit/5
    // Handles the form submission and updates the existing room in the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNumber,RoomType,Price")] Room room)
    {
        if (id != room.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(room); // Marks the entity as modified
                await _context.SaveChangesAsync(); // Executes the UPDATE command
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rooms.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(room);
    }

    public async Task<IActionResult> Options(int? id)
    {
        return View(await _context.Rooms.FindAsync(id));
    }
    
    public async Task<IActionResult> Onboard()
    {
        var units = await _context.Rooms
        .Where(x => !x.IsOccupied)
        .ToListAsync();

        var onboard = new BookingVM
        {
            Rooms = units
        };
        return View(onboard);
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

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> BookUnit([FromForm] BookingVM model)
        // {
        //     // We only validate the fields required for the Tenant and the Room link
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             // 1. Map the client details to your Tenant/Client entity
        //             var newTenant = new Tenant
        //             {
        //                 FullName = model.FullName, // Map to your specific DB field name
        //                 Email = model.Email,
        //                 IdNumber = model.IdNumber,
        //                 CreatedAt = DateTime.UtcNow
        //             };

        //             // 2. Map the booking/room info
        //             var booking = new BookingVM
        //             {
        //                 RoomId = model.Unit, // "unit" radio value from form
        //                 BookingType = model.StayType, // "short" or "long"
        //                 Tenant = newTenant,
        //                 Status = "Pending", // Default status
        //                 IsPaymentVerified = true 
        //             };

        //             _context.Tenants.Add(newTenant);
        //             _context.RoomBookings.Add(booking);
                    
        //             await _context.SaveChangesAsync();

        //             // Return a success response (or JSON for your modal)
        //             return Json(new { success = true, message = "Booking captured successfully" });
        //         }
        //         catch (Exception ex)
        //         {
        //             // Log error
        //             return Json(new { success = false, message = "Internal server error occurred." });
        //         }
        //     }

        //     // If validation fails, return the errors
        //     return Json(new { success = false, message = "Invalid data submitted." });
        // }

    // POST: Rooms/Delete/5
    // Executes the deletion of the room from the database.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room); // Marks the entity for deletion
            await _context.SaveChangesAsync(); // Executes the DELETE command
        }
        return RedirectToAction(nameof(Index));
    }
}