using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces; // Added this to fix the error
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Implementations
{
    // Now the compiler will recognize that BatchService implements IBatchService
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;

        public BatchService(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<IEnumerable<Batch>> GetAllBatchesAsync()
        {
            return await _batchRepository.GetAllBatchesAsync();
        }

        public async Task UpdateRemainingQuantity(int batchId, int quantitySold)
        {
            var batch = await _batchRepository.GetBatchByIdAsync(batchId);
            if (batch != null)
            {
                batch.RemainingQuantity -= quantitySold;

                // Set status to OutOfStock if quantity reaches 0 or less
                if (batch.RemainingQuantity <= 0)
                {
                    batch.RemainingQuantity = 0; // Prevent negative stock
                    batch.Status = BatchStatus.OutOfStock;
                }

                await _batchRepository.UpdateBatchAsync(batch);
            }
        }
    }
}