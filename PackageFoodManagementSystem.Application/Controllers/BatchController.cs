using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Controllers
{
    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        // GET: Batch/Index
        public async Task<IActionResult> Index()
        {
            var batches = await _batchService.GetAllBatchesAsync();
            return View(batches);
        }

        // POST: Batch/CreateAjax
        // This handles the AJAX request from your AddProduct.cshtml modal
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
                // Set initial logic: Remaining quantity starts equal to Initial Quantity
                batch.RemainingQuantity = batch.InitialQuantity;

                await _batchService.AddBatchAsync(batch);
                return Json(new { success = true, message = "Product saved successfully!" });
            }

            return Json(new { success = false, message = "Validation failed.", errors = ModelState });
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