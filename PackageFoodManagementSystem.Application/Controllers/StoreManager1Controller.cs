using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class StoreManager1Controller : Controller
{
    private readonly IProductService _service;
    public StoreManager1Controller(IProductService service) => _service = service;

    public IActionResult AddProduct()
    {
        IEnumerable<object> products = _service.GetAllProducts();
        return View(products); // Pass the list to the view
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _service.CreateProduct(product);
        return RedirectToAction("AddProduct");
    }
}