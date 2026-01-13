using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
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
                    Role = "User" // ✅ default role
                };

                await _userRepository.AddAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);

                return user.Id;

            }
        }
}
