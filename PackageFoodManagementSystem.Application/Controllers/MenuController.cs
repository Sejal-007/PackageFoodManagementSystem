<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿

using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
>>>>>>> 3a07ba1 (addedbackendto prd model)

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductService _service;

        public MenuController(IProductService service) => _service = service;

        public IActionResult Index()
        {
            // This pulls the same data the manager just saved
            var products = _service.GetAllProducts();
            return View(products);
        }
    }
}
