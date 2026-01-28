using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Includes the Product details so you can see the Product Name in the list
            return await _context.Batches
                .Include(b => b.Product)
                .ToListAsync();
        }

        public async Task<Batch?> GetBatchByIdAsync(int id)
        {
            return await _context.Batches
                .Include(b => b.Product)
                .FirstOrDefaultAsync(b => b.BatchId == id);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByProductIdAsync(int productId)
        {
            return await _context.Batches
                .Where(b => b.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddBatchAsync(Batch batch)
        {
            await _context.Batches.AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}