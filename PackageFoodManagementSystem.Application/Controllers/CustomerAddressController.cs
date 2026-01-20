//using Microsoft.AspNetCore.Mvc;
//using PackageFoodManagementSystem.Repository.Models;
//using PackageFoodManagementSystem.Services.Interfaces;

//namespace PackageFoodManagementSystem.Application.Controllers
//{
//    public class CustomerAddressController : Controller
//    {
//        private readonly ICustomerAddressService _service;
//        public CustomerAddressController(ICustomerAddressService service)
//        {
//            _service = service;
//        }

//        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

//        public async Task<IActionResult> Details(int id)
//        {
//            var address = await _service.GetByIdAsync(id);
//            if (address == null) return NotFound();
//            return View(address);
//        }

//        public IActionResult Create() => View();

//        [HttpPost]
//        public async Task<IActionResult> Create(CustomerAddress address)
//        {
//            if (ModelState.IsValid)
//            {
//                await _service.AddAsync(address);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(address);
//        }

//        public async Task<IActionResult> Edit(int id)
//        {
//            var address = await _service.GetByIdAsync(id);
//            if (address == null) return NotFound();
//            return View(address);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(CustomerAddress address)
//        {
//            if (ModelState.IsValid)
//            {
//                await _service.UpdateAsync(address);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(address);
//        }

//        public async Task<IActionResult> Delete(int id)
//        {
//            var address = await _service.GetByIdAsync(id);
//            if (address == null) return NotFound();
//            return View(address);
//        }

//        [HttpPost, ActionName("Delete")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            await _service.DeleteAsync(id);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}


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