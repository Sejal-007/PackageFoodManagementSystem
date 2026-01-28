using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(
            string name,
            string mobileNumber,
            string email,
            string password,
            CancellationToken cancellationToken = default);

        Task<UserAuthentication?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<UserAuthentication?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UserAuthentication>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task AddUserAsync(UserAuthentication user, CancellationToken cancellationToken = default);   // ✅ NEW
        Task UpdateUserAsync(UserAuthentication user, CancellationToken cancellationToken = default); // ✅ NEW
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);                  // ✅ NEW
        Task<int> CountUsersByRoleAsync(string role, CancellationToken cancellationToken = default);  // ✅ NEW
    }
}
