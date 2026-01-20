//using PackageFoodManagementSystem.Repository.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using PackageFoodManagementSystem.Services.Interfaces;

//public class StoreManager1Controller : Controller
//{
//    private readonly IProductService _service;

//    public StoreManager1Controller(IProductService service)
//    {
//        _service = service;
//    }

//    // GET: Displays the list of products for the Manager
//    public IActionResult AddProduct()
//    {
//        //var products = _service.GetAllProducts();
//        return View();
//    }

//    // POST: Handles the form submission from the Manager

//    [HttpPost]
//    public IActionResult Create(Product product)
//    {
//        // 1. Validate the data coming from the form
//        if (ModelState.IsValid)
//        {
//            // 2. This calls your Service, which calls your Repository to save to SQL
//            _service.CreateProduct(product);

//            // 3. After saving, it reloads the page to show the updated table
//            return RedirectToAction("AddProduct");
//        }

//        // If there is an error, reload the page with the existing products
//        var products = _service.GetAllProducts();
//        return View("AddProduct", products);
//    }

//}

using PackageFoodManagementSystem.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http; // Required for IFormFile
using System.IO; // Required for MemoryStream

public class StoreManager1Controller : Controller
{
    private readonly IProductService _service;

    public StoreManager1Controller(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        var products = _service.GetAllProducts();
        return View(products);
    }

    [HttpPost]
    public IActionResult Create(Product product, IFormFile imageFile)
    {
        // 1. Convert the image file to a Base64 string if a file was uploaded
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();

                // Formats the string so the <img> tag knows it is an image
               // product.ImageData = "data:image/png;base64," + Convert.Base64String(fileBytes);
            }
        }

        // 2. Validate and Save
        if (ModelState.IsValid)
        {
            _service.CreateProduct(product);
            return RedirectToAction("AddProduct");
        }

        // If something went wrong, reload the list and show the view
        var products = _service.GetAllProducts();
        return View("AddProduct", products);
    }
}