using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Implementations
{
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

        public async Task AddBatchAsync(Batch batch)
        {
            // Adding the batch to the repository
            await _batchRepository.AddBatchAsync(batch);

            // Crucial: Ensure SaveChanges() is called in the Repository 
            // or here if your unit of work pattern requires it.
            await _batchRepository.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            await _batchRepository.UpdateBatchAsync(batch);
        }

        public async Task DeleteBatchAsync(int id)
        {
            await _batchRepository.DeleteBatchAsync(id);
        }

        // Replaced UpdateRemainingQuantity with this method to match IBatchService
        public async Task UpdateQuantity(int batchId, int quantitySold)
        {
            var batch = await _batchRepository.GetBatchByIdAsync(batchId);
            if (batch != null)
            {
                batch.Quantity -= quantitySold;

                if (batch.Quantity < 0)
                {
                    batch.Quantity = 0; // Prevent negative stock
                }

                await _batchRepository.UpdateBatchAsync(batch);
            }
        }
    }
}