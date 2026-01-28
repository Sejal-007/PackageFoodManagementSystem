using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Linq; // Added for filtering logic

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductService _service;

        public MenuController(IProductService service) => _service = service;

        // GET: Menu/Index
        public IActionResult Index(string? category)
        {
            // Fetch all products from the service
            var products = _service.GetAllProducts();

            // If a category is selected, filter the list
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category).ToList();
                ViewBag.SelectedCategory = category; // Helps UI highlight selection
            }

            return View(products);
        }

        // GET: Menu/Details/5
        public IActionResult Details(int id)
        {
            // We use the service to find the product. 
            // Ensure your ProductService implementation handles the .Include(p => p.Batches)
            var product = _service.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}