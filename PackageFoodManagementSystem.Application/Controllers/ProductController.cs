using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using PackageFoodManagementSystem.Repository.Data; // <--- Added to recognize ApplicationDbContext

using System.Threading.Tasks;

using System.Linq;

namespace PackageFoodManagementSystem.Application.Controllers

{

    public class ProductController : Controller

    {

        private readonly IProductService _productService;

        private readonly ApplicationDbContext _context; // <--- 1. Declared the context

        // 2. Updated constructor to inject both the Service and the Context

        public ProductController(IProductService productService, ApplicationDbContext context)

        {

            _productService = productService;

            _context = context;

        }

        // CUSTOMER VIEW: Displays all products

        [HttpGet]

        public IActionResult Index()

        {

            var products = _productService.GetMenuForCustomer();

            return View(products);

        }

        // MANAGER VIEW: Shows the 'Add Product' form

        [HttpGet]

        public IActionResult Create()

        {

            // Fetches categories (1=Veg, 2=Dairy, 3=Snacks, etc.) for the dropdown

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            return View();

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Product product)

        {

            if (ModelState.IsValid)

            {

                // Link the Name to the ID automatically

                var categoryData = await _context.Categories

                    .FirstOrDefaultAsync(c => c.CategoryName == product.Category);

                if (categoryData != null)

                {

                    // Update the integer ID based on the string name selection

                    product.CategoryId = categoryData.CategoryId;

                }

                _context.Products.Add(product);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryName", "CategoryName");

            return View(product);

        }

    }

}
