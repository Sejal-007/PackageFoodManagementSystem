using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

public class StoreManager1Controller : Controller
{
    private readonly IProductService _service;

    public StoreManager1Controller(IProductService service)
    {
        _service = service;
    }

    // 1. MAIN LIST PAGE: This is what opens when clicking "Products"
    [HttpGet]
    public IActionResult Index()
    {
        var products = _service.GetAllProducts();
        return View(products);
    }

    // 2. ADD PRODUCT PAGE: Only shows your beautiful purple form
    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    // 3. CREATE LOGIC: Handles the Save button
    [HttpPost]
    public IActionResult Create(Product product, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                product.ImageData = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
        }

        if (ModelState.IsValid)
        {
            _service.CreateProduct(product);
            // Redirect back to the LIST (Index) after saving
            return RedirectToAction("Index");
        }

        return View("AddProduct", product);
    }

    // 4. DELETE LOGIC: Triggered by the trash icon
    //[HttpPost]
    //public IActionResult Delete(int id)
    //{
    //    _service.DeleteProduct(id); // Ensure your service has a Delete method
    //    return RedirectToAction("Index");
    //}
}

//using PackageFoodManagementSystem.Repository.Models;
//using Microsoft.AspNetCore.Mvc;
//using PackageFoodManagementSystem.Services.Interfaces;
//using Microsoft.AspNetCore.Http; // Required for IFormFile
//using System.IO; // Required for MemoryStream

//public class StoreManager1Controller : Controller
//{
//    private readonly IProductService _service;

//    public StoreManager1Controller(IProductService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public IActionResult AddProduct()
//    {
//        var products = _service.GetAllProducts();
//        return View(products);
//    }

//    [HttpPost]
//    [HttpPost]
//    public IActionResult Create(Product product, IFormFile imageFile)
//    {
//        if (imageFile != null && imageFile.Length > 0)
//        {
//            using (var ms = new MemoryStream())
//            {
//                imageFile.CopyTo(ms);
//                byte[] fileBytes = ms.ToArray();

//                // Fix: Use ToBase64String
//                product.ImageData = "data:image/png;base64," + Convert.ToBase64String(fileBytes);
//            }
//        }

//        if (ModelState.IsValid)
//        {
//            _service.CreateProduct(product);
//            return RedirectToAction("AddProduct");
//        }

//        return View("AddProduct", _service.GetAllProducts());
//    }
//}