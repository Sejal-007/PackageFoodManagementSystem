using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces; // <--- Add this line!
namespace PackageFoodManagementSystem.Application.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // CUSTOMER VIEW: Displays all products
        [HttpGet]
        public IActionResult Index()
        {
            var products = _productService.GetMenuForCustomer();
            return View(products); // Sends the list to Index.cshtml
        }

        // MANAGER VIEW: Shows the 'Add Product' form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // MANAGER ACTION: Saves the product and redirects
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateNewProduct(product);
                // After adding, go to the Customer Index to see the result
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
