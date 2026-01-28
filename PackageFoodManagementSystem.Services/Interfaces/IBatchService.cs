using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IBatchService
    {
        Task<IEnumerable<Batch>> GetAllBatchesAsync();
        Task AddBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);
        // Changed parameter name for clarity
        Task UpdateQuantity(int batchId, int quantityChange);
    }
}