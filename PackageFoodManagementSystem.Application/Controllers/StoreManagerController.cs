using Microsoft.AspNetCore.Mvc;

namespace PackageFoodManagementSystem.Application.Controllers
{
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
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult Inventory()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Compliance()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}