using Microsoft.AspNetCore.Mvc;

namespace PackageFoodManagementSystem.Controllers
{
    // In StoreController.cs
    public class StoreController : Controller // Rename this
    {
        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
    }
}