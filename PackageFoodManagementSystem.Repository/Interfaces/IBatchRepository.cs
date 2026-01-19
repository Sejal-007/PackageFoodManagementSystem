using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetAllBatchesAsync();
        Task<Batch?> GetBatchByIdAsync(int id);
        Task<IEnumerable<Batch>> GetBatchesByProductIdAsync(int productId);
        Task AddBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);

        // This satisfies the BatchRepository error from earlier
        Task SaveChangesAsync();
    }
}