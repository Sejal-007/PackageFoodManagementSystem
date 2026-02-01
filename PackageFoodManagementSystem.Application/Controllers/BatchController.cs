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
        private readonly IProductService _productService;
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
            ViewBag.Products = _productService.GetAllProducts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                // Changed from .Batch to .Batches
                _context.Batches.Add(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", batch.ProductId);
            return View(batch);
        }

        // GET: Batch/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            // Changed from .Batch to .Batches
            var batch = _context.Batches.Find(id);
            if (batch == null) return NotFound();

            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", batch.ProductId);
            return View(batch);
        }

        // POST: Batch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Batch batch)
        {
            if (id != batch.BatchId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Changed from .Batch to .Batches
                    _context.Batches.Update(batch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.BatchId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", batch.ProductId);
            return View(batch);
        }

        // POST: Batch/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Changed from .Batch to .Batches
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            // Changed from .Batch to .Batches
            return _context.Batches.Any(e => e.BatchId == id);
        }

        #region Ajax Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromBody] Batch batch)
        {
            if (batch == null) return Json(new { success = false, message = "Invalid data." });
            if (ModelState.IsValid)
            {
                await _batchService.AddBatchAsync(batch);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState });
        }

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
        #endregion
    }
}