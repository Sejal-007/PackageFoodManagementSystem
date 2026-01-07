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
    //public async Task<IActionResult> Index()
    //    {
    //        var activeProducts = await _context.Products
    //                                    .Where(p => p.Status == "ACTIVE")
    //                                    .ToListAsync();
    //        return View(activeProducts);
    //    }
    //}
}
