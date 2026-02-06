using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System;

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

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            var model = new Batch
            {
                Categories = categories,
                ManufactureDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                _context.Batches.Add(batch);
                await _context.SaveChangesAsync();

                // Recalculate Product Quantity
                await UpdateProductTotalQuantity(batch.ProductId);

                return RedirectToAction(nameof(Index));
            }

            batch.Categories = _context.Categories.ToList();
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
                    // Track original ProductId in case the user changes which product this batch belongs to
                    var oldBatch = await _context.Batches.AsNoTracking().FirstOrDefaultAsync(b => b.BatchId == id);
                    int oldProductId = oldBatch.ProductId;

                    _context.Batches.Update(batch);
                    await _context.SaveChangesAsync();

                    // Update current product
                    await UpdateProductTotalQuantity(batch.ProductId);

                    // If ProductId was changed, update the old product's total too
                    if (oldProductId != batch.ProductId)
                    {
                        await UpdateProductTotalQuantity(oldProductId);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.BatchId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(batch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                int productId = batch.ProductId;
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();

                // Recalculate after removal
                await UpdateProductTotalQuantity(productId);
            }
            return RedirectToAction(nameof(Index));
        }

        // --- NEW HELPER METHOD ---
        private async Task UpdateProductTotalQuantity(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                // Calculate Sum of all batches for this product
                int totalBatchQuantity = await _context.Batches
                    .Where(b => b.ProductId == productId)
                    .SumAsync(b => b.Quantity);

                // Update the product table
                product.Quantity = totalBatchQuantity;
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        private bool BatchExists(int id) => _context.Batches.Any(e => e.BatchId == id);
    }
}