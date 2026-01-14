using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IBatchService
    {
        /// <summary>
        /// Retrieves all food batches from the system.
        /// </summary>
        Task<IEnumerable<Batch>> GetAllBatchesAsync();

        /// <summary>
        /// Logic to deduct quantity when items are sold or moved.
        /// Automatically updates status to OutOfStock if quantity reaches 0.
        /// </summary>
        Task UpdateRemainingQuantity(int batchId, int quantitySold);

        // You can add more business-specific methods here later, such as:
        // Task<IEnumerable<Batch>> GetExpiredBatchesAsync();
        // Task<IEnumerable<Batch>> GetLowStockBatchesAsync(int threshold);
    }
}