using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<IActionResult> Index()
        {
            var batches = await _batchService.GetAllBatchesAsync();
            return View(batches);
        }

        // GET: Batch/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList(); // Fetch from DB

            var model = new Batch
            {
                Categories = categories, // This prevents the NullReferenceException
                ManufactureDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6)
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetProductsByCategory(int categoryId)
        {
            // Fetching from DB based on the Category ID selected in the UI
            var products = _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new {
                    productId = p.ProductId,
                    productName = p.ProductName
                })
                .ToList();

            return Json(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                _context.Batches.Add(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate if validation fails
            batch.Categories = _context.Categories.ToList();
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.CategoryId == batch.CategoryId), "ProductId", "ProductName", batch.ProductId);
            return View(batch);
        }

        public IActionResult Edit(int id)
        {
            var batch = _context.Batches.Find(id);
            if (batch == null) return NotFound();

            batch.Categories = _context.Categories.ToList();
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.CategoryId == batch.CategoryId), "ProductId", "ProductName", batch.ProductId);

            return View(batch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Batch batch)
        {
            if (id != batch.BatchId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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

            batch.Categories = _context.Categories.ToList();
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.CategoryId == batch.CategoryId), "ProductId", "ProductName", batch.ProductId);
            return View(batch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
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
            return Json(new { success = false, errors = "Model state is invalid" });
        }
        #endregion
    }
}