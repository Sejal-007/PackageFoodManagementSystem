using System.Threading;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interface;

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
    }
}
