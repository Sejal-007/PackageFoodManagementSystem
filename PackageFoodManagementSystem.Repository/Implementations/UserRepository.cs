using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            await _db.Set<T>().AddAsync(entity, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserAuthentication?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _db.UserAuthentications.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<UserAuthentication?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _db.UserAuthentications.FindAsync(new object[] { id }, cancellationToken);

        public async Task<List<UserAuthentication>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _db.UserAuthentications.ToListAsync(cancellationToken);

        public async Task UpdateAsync(UserAuthentication user, CancellationToken cancellationToken = default)
        {
            _db.UserAuthentications.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _db.UserAuthentications.FindAsync(new object[] { id }, cancellationToken);
            if (user != null)
            {
                _db.UserAuthentications.Remove(user);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> CountByRoleAsync(string role, CancellationToken cancellationToken = default)
            => await _db.UserAuthentications.CountAsync(u => u.Role == role, cancellationToken);
    }
}
