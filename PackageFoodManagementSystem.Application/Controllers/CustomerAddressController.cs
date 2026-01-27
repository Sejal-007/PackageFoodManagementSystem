

using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class CustomerAddressController : Controller
    {
        private readonly ICustomerAddressService _service;

        public CustomerAddressController(ICustomerAddressService service)
        {
            _service = service;
        }

        // 1. Load the list of addresses
        public async Task<IActionResult> Index()
        {
            var addresses = await _service.GetAllAsync();
            return View(addresses);
        }

        // 2. Add New Address (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerAddress address)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(address);
                TempData["Message"] = "Address added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await _service.GetAllAsync());
        }

        // 3. Delete Address (POST for AJAX)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(); // Returns 200 OK for the JS fetch
        }
    }
}