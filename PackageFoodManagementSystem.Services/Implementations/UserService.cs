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

        public async Task<UserAuthentication?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterUserAsync(UserAuthentication user)
        {
            try
            {
                user.Password = PasswordHelper.HashPassword(user.Password);
                user.Role = "User";

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                var customer = new Customer
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.MobileNumber,
                    Status = "Active",
                    Addresses = new List<CustomerAddress>()
                };

                await _userRepository.AddAsync(customer);
                await _userRepository.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public Task<List<UserAuthentication>> GetAllUsersAsync()
            => _userRepository.GetAllUsersAsync();

        public async Task<(bool Success, string? ErrorMessage)> UpdateUserAsync(UserAuthentication user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (existingUser == null) return (false, "User not found");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.Role = user.Role;

            if (!string.IsNullOrEmpty(user.Password))
                existingUser.Password = PasswordHelper.HashPassword(user.Password);

            await _userRepository.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        public Task<(int TotalCustomers, int TotalStoreManagers, int TotalOrders)> GetAdminDashboardStatsAsync()
            => _userRepository.GetDashboardStatsAsync();
    }
}
