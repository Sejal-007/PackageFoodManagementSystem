<<<<<<< HEAD
=======
<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
>>>>>>> cea734f18942ffda35f30bb5396d6b26c0f611c6
﻿

using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 3a07ba1 (addedbackendto prd model)
=======
>>>>>>> 3a07ba1 (addedbackendto prd model)
>>>>>>> cea734f18942ffda35f30bb5396d6b26c0f611c6

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
