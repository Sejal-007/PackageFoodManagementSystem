using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PackageFoodManagementSystem.Repository.Implementations
{
    public class BatchRepository : IBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public BatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Batch>> GetAllBatchesAsync()
        {
            return await _context.Batch.Include(b => b.Product).ToListAsync();
        }

        public async Task<Batch?> GetBatchByIdAsync(int id)
        {
            return await _context.Batch.Include(b => b.Product)
                                       .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByProductIdAsync(int productId)
        {
            return await _context.Batch.Where(b => b.Product.Id == productId).ToListAsync();
        }

        public async Task AddBatchAsync(Batch batch)
        {
            await _context.Batch.AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            _context.Batch.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(int id)
        {
            var batch = await _context.Batch.FindAsync(id);
            if (batch != null)
            {
                _context.Batch.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }
    }
}
