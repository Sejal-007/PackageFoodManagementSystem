using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Interfaces;

namespace PackageFoodManagementSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> CreateUserAsync(
            string name,
            string mobileNumber,
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = new UserAuthentication
            {
                Name = name,
                Email = email,
                MobileNumber = mobileNumber,
                Password = PasswordHelper.HashPassword(password),
                Role = "User"
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return user.Id;
        }

        public Task<UserAuthentication?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
            => _userRepository.GetByEmailAsync(email, cancellationToken);

        public Task<UserAuthentication?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
            => _userRepository.GetByIdAsync(id, cancellationToken);

        public Task<List<UserAuthentication>> GetAllUsersAsync(CancellationToken cancellationToken = default)
            => _userRepository.GetAllAsync(cancellationToken);

        public async Task AddUserAsync(UserAuthentication user, CancellationToken cancellationToken = default)
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateUserAsync(UserAuthentication user, CancellationToken cancellationToken = default)
            => _userRepository.UpdateAsync(user, cancellationToken);

        public Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
            => _userRepository.DeleteAsync(id, cancellationToken);

        public Task<int> CountUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
            => _userRepository.CountByRoleAsync(role, cancellationToken);
    }
}
