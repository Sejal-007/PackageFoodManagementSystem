using Microsoft.AspNetCore.Mvc;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class StoreManagerController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Orders()
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
    }
}