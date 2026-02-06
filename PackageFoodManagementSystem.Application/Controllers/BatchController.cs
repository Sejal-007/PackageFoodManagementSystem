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

        // --- AJAX METHOD FOR DROPDOWN ---
        [HttpGet]
        public JsonResult GetProductsByCategory(int categoryId)
        {
            var products = _context.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive == true)
                .Select(p => new {
                    productId = p.ProductId,
                    productName = p.ProductName
                })
                .ToList();
            return Json(products);
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

                // Recalculate Product Quantity automatically
                await UpdateProductTotalQuantity(batch.ProductId);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
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
                await UpdateProductTotalQuantity(productId);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task UpdateProductTotalQuantity(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                int totalBatchQuantity = await _context.Batches
                    .Where(b => b.ProductId == productId)
                    .SumAsync(b => b.Quantity);

                product.Quantity = totalBatchQuantity;
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        private bool BatchExists(int id) => _context.Batches.Any(e => e.BatchId == id);
    }
}