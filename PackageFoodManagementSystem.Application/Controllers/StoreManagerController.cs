using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data; 
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Application.Controllers
{
    [Authorize(Roles = "StoreManager")]
    public class StoreManagerController : Controller
    {
        // Resolved CS0103: Context is now defined at the class level
        private readonly ApplicationDbContext _context;

        // Dependency Injection via Constructor
        public StoreManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Standard Navigation Actions ---
        public IActionResult Home() => View();
        public IActionResult Orders() => View();
        public IActionResult Inventory() => View();
        public IActionResult Settings() => View(); // This loads image_2d7569.png

        // --- Edit Profile Logic ---

        [HttpGet]
        public async Task<IActionResult> EditProfile(int id)
        {
            // Fetch the specific record from the database
            var data = await _context.Products.FindAsync(id); 
            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                // _context is now available for the update operation
                _context.Update(product);
                await _context.SaveChangesAsync();
                
                // Redirect back to the Settings dashboard
                return RedirectToAction("Settings");
            }
            // If validation fails, stay on the edit page
            return View("EditProfile", product);
        }
    }
}