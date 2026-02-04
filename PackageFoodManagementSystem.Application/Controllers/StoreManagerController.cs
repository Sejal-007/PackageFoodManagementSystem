using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.DTOs;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Application.Controllers
{

    [Authorize(Roles = "StoreManager")]
    public class StoreManagerController : Controller
    {
        //    [HttpPost]
        //    public async Task<IActionResult> Create(Product product)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(product);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }
        //        return View(product);
        //    }

        private readonly ApplicationDbContext _context;


        public StoreManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }


        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult OrdersDashboard()
        {
            var today = DateTime.Today;

            ViewBag.TodayOrders = _context.Orders
                .Count(o => o.OrderDate >= today);

            ViewBag.PendingOrders = _context.Orders
                .Count(o => o.OrderStatus == "Pending" || o.OrderStatus == "Placed");

            ViewBag.CompletedOrders = _context.Orders
                .Count(o => o.OrderStatus == "Confirmed" || o.OrderStatus == "Delivered");

            ViewBag.CancelledOrders = _context.Orders
                .Count(o => o.OrderStatus == "Cancelled");

            return View();
        }

        public IActionResult Orders(string status)

        {

            var orders = _context.Orders

                .Include(o => o.Customer)   // or Customer

                .AsQueryable();

            if (!string.IsNullOrEmpty(status))

            {

                if (status == "Today")

                {

                    var today = DateTime.Today;

                    orders = orders.Where(o => o.OrderDate >= today);

                }

                else if (status == "Completed")

                {

                    orders = orders.Where(o =>

                        o.OrderStatus == "Confirmed" || o.OrderStatus == "Delivered");

                }

                else

                {

                    orders = orders.Where(o => o.OrderStatus == status);

                }

            }

            return View(orders.ToList());

        }

        public IActionResult Inventory()
        {
            return View();
        }
        public IActionResult Reports()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            // Use 'StoreReportDto' instead of 'StoreReportViewModel'
            var reportData = new StoreReportDto
            {
                TotalRevenue = _context.Orders
                    .Where(o => o.OrderDate >= thirtyDaysAgo && o.OrderStatus != "Cancelled")
                    .Sum(o => o.TotalAmount),

                TotalOrders = _context.Orders
                    .Count(o => o.OrderDate >= thirtyDaysAgo),

                TopProducts = _context.OrderItems
                    .Include(oi => oi.Product)
                    .GroupBy(oi => oi.Product.ProductName) // Verified 'Name' from your SQL screenshot
                    .Select(g => new TopProductDto
                    {
                        ProductName = g.Key,
                        QuantitySold = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.Subtotal)
                    })
                    .OrderByDescending(x => x.QuantitySold)
                    .Take(5)
                    .ToList()
            };

            return View(reportData);
        }
        public IActionResult Compliance()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }

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