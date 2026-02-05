using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<UserAuthentication?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<UserAuthentication?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UserAuthentication>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(UserAuthentication user, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountByRoleAsync(string role, CancellationToken cancellationToken = default);
        Task<UserAuthentication?> GetUserByEmailAsync(string email);
        Task<UserAuthentication?> GetUserByIdAsync(int id);
        Task<List<UserAuthentication>> GetAllUsersAsync();
        Task DeleteUserAsync(int id);
        Task<(int TotalCustomers, int TotalStoreManagers, int TotalOrders)> GetDashboardStatsAsync();
    }
}
