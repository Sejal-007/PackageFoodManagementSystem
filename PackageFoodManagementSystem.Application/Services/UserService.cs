using PackageFoodManagementSystem.Application.Repositories;
using PackageFoodManagementSystem.Application.Services;
using PackageFoodManagementSystem.Services;
using PackageFoodManagementSystem.Application.Helpers;
using PackageFoodManagementSystem.Application.Models;
using PackageFoodManagementSystem.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Application.Services
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
                    Role = "User" // ✅ default role
                };

                await _userRepository.AddAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);

                return user.Id;

            }
        }
}
