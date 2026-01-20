using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            await _service.AddAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId) return BadRequest();

            if (!ModelState.IsValid) return View(customer);

            await _service.UpdateAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
