using Microsoft.AspNetCore.Mvc;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
