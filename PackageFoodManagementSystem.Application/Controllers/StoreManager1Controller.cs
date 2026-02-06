using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

public class StoreManager1Controller : Controller

{

    private readonly IProductService _service;

    private readonly ApplicationDbContext _context;

    // 2. Create the constructor to "inject" the context


    public StoreManager1Controller(IProductService service, ApplicationDbContext context)

    {

        _service = service;

        _context = context;

    }

    // 1. MAIN LIST PAGE: This is what opens when clicking "Products"

    [HttpGet]

    public IActionResult Index()

    {

        var products = _service.GetAllProducts();

        return View(products);

    }

    // Add this so the page opens when you click the button

    [HttpGet]

    public IActionResult AddProduct()

    {

        return View();

    }

    // 2. ADD PRODUCT PAGE: Only shows your beautiful purple form

    [HttpPost]

    public async Task<IActionResult> AddProduct(Product product)

    {

        // Look up the ID from the Categories table using the string name

        var categoryData = await _context.Categories

            .FirstOrDefaultAsync(c => c.CategoryName == product.Category);

        if (categoryData != null)

        {

            // Automatically set the ID so the database stays linked

            product.CategoryId = categoryData.CategoryId;

        }

        if (ModelState.IsValid)

        {

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product added successfully!";

            return RedirectToAction("Index");

        }

        return View(product);

    }

    // 3. CREATE LOGIC: Handles the Save button

    [HttpPost]

    public async Task<IActionResult> Create(Product product, IFormFile imageFile)

    {

        // 1. Link Category Name to CategoryId automatically

        var categoryData = await _context.Categories

            .FirstOrDefaultAsync(c => c.CategoryName == product.Category);

        if (categoryData != null)

        {

            product.CategoryId = categoryData.CategoryId; // Sets 1 for Veg, 3 for Snacks, etc.

        }

        // 2. Handle the Image File conversion

        if (imageFile != null && imageFile.Length > 0)

        {

            using (var ms = new MemoryStream())

            {

                imageFile.CopyTo(ms);

                product.ImageData = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

            }

        }

        // 3. Save everything to the database

        if (ModelState.IsValid)

        {

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product added successfully!";

            return RedirectToAction("Index");

        }

        // If validation fails, stay on the Add page

        return View("AddProduct", product);

    }

    // 4. EDIT PAGE: Shows the form with existing data

    [HttpGet]

    // GET: StoreManager1/Edit/5

    public IActionResult Edit(int id)

    {

        // This fetches the SPECIFIC product from the DB

        var product = _service.GetProductById(id);

        if (product == null)

        {

            return NotFound();

        }

        // This sends the data to the Edit.cshtml page so the fields are filled

        return View(product);

    }

    // 5. UPDATE LOGIC: Saves the changes from the Edit form

    [HttpPost]

    public IActionResult Edit(Product product, IFormFile? imageFile)

    {

        // 1. Fetch the actual record currently in the DB

        var productInDb = _service.GetProductById(product.ProductId);

        if (productInDb == null) return NotFound();

        // 2. Map EVERY column from your list

        productInDb.ProductName = product.ProductName;

        productInDb.Price = product.Price;

        productInDb.Category = product.Category;

        productInDb.IsActive = product.IsActive; // Added IsActive

        productInDb.Quantity = product.Quantity;

        // 3. Handle Image logic (Only update if a new file is chosen)

        if (imageFile != null && imageFile.Length > 0)

        {

            using var ms = new MemoryStream();

            imageFile.CopyTo(ms);

            productInDb.ImageData = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

        }

        // 4. Save the modified object

        if (ModelState.IsValid)

        {

            _service.UpdateProduct(productInDb);

            TempData["SuccessMessage"] = "Product updated successfully!"; // <--- Add this

            return RedirectToAction("Index");

        }

        return View(product);

    }

    // 6. DELETE LOGIC: Triggered by the trash icon

    [HttpPost]

    public IActionResult Delete(int id)

    {

        _service.DeleteProduct(id);

        TempData["DeleteMessage"] = "Product deleted successfully!"; // <--- Add this

        return RedirectToAction("Index");

    }

}
