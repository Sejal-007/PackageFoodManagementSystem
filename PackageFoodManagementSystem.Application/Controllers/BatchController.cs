using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackageFoodManagementSystem.Controllers
{
    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IProductService _productService; // Added to fetch products for the dropdown
        private readonly ApplicationDbContext _context;

        public BatchController(IBatchService batchService, IProductService productService, ApplicationDbContext context)
        {
            _batchService = batchService;
            _productService = productService;
            _context = context;
        }

        // GET: Batch/Index
        public async Task<IActionResult> Index()
        {
            var batches = await _batchService.GetAllBatchesAsync();
            return View(batches);
        }

        // GET: Batch/Create
        public IActionResult Create()
        {
            // Fetch products so the user can select which product this batch belongs to
            ViewBag.Products = _productService.GetAllProducts();
            return View();
        }

        // POST: Batch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                // Using the service to maintain consistency across the app
                await _batchService.AddBatchAsync(batch);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = _productService.GetAllProducts();
            return View(batch);
        }

        // POST: Batch/CreateAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromBody] Batch batch)
        {
            if (batch == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            if (ModelState.IsValid)
            {
                await _batchService.AddBatchAsync(batch);
                return Json(new { success = true, message = "Batch saved successfully!" });
            }

            return Json(new { success = false, message = "Validation failed.", errors = ModelState });
        }

        // POST: Batch/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Batch batch)
        {
            if (id != batch.BatchId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _batchService.UpdateBatchAsync(batch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(batch);
        }

        // POST: Batch/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                await _batchService.UpdateBatchAsync(batch);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // POST: Batch/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _batchService.DeleteBatchAsync(id);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}