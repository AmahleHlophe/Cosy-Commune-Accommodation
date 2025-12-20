using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CosyCommune.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MaintenanceController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Maintenance/Index (Tenant Dashboard)
        public async Task<IActionResult> Index()
        {
            // In a real app, filter by the logged-in User's ID
            var requests = await _context.Maintenance
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
                
            return View(requests);
        }

        public IActionResult LogMaintenanceFlow()
        {
            return View();
        }

        // GET: Maintenance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Maintenance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maintenance request)
        {
            if (ModelState.IsValid)
            {
                // 1. Handle File Upload (Image)
                if (request.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(request.ImageFile.FileName);
                    string extension = Path.GetExtension(request.ImageFile.FileName);
                    
                    // Create unique filename to prevent overwriting
                    request.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmddssfff") + extension;
                    
                    string path = Path.Combine(wwwRootPath + "/uploads/maintenance/", fileName);
                    
                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await request.ImageFile.CopyToAsync(fileStream);
                    }
                }

                // 2. Set default metadata
                request.CreatedAt = DateTime.UtcNow;
                request.Status = "Pending";
                // request.TenantId = GetCurrentUserId(); // Logic to get logged in user

                _context.Add(request);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Maintenance/Track/5
        public async Task<IActionResult> Track(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.Maintenance
                .FirstOrDefaultAsync(m => m.Id == id);

            if (request == null) return NotFound();

            return View(request);
        }

        // POST: Maintenance/UpdateStatus (Admin/Technician Action)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var request = await _context.Maintenance.FindAsync(id);
            if (request == null) return Json(new { success = false });

            request.Status = status;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}