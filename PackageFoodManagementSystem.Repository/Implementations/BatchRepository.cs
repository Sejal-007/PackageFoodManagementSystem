using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System;
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
            return await _context.Batches.Include(b => b.Product).ToListAsync();
        }

        public async Task<Batch?> GetBatchByIdAsync(int id)
        {
            // Changed back to .Id because your Batch model likely uses 'Id'
            return await _context.Batches.Include(b => b.Product)
                                         .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByProductIdAsync(int productId)
        {
            // Keeping ProductId here because the error confirmed Product uses ProductId
            return await _context.Batches.Where(b => b.Product.ProductId == productId).ToListAsync();
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