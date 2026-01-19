using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IBatchService
    {
        Task<IEnumerable<Batch>> GetAllBatchesAsync();

        // These three lines fix your current errors:
        Task AddBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);

        Task UpdateRemainingQuantity(int batchId, int quantitySold);
    }
}