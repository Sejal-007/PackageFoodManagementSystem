//using PackageFoodManagementSystem.Services.Helpers;
//using PackageFoodManagementSystem.Repository.Models;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using PackageFoodManagementSystem.Services.Interfaces;
//using PackageFoodManagementSystem.Repository.Interfaces;

//namespace PackageFoodManagementSystem.Services.Implementations
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository _userRepository;

//        public UserService(IUserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        public async Task<int> CreateUserAsync(
//            string name,
//            string mobileNumber,
//            string email,
//            string password,
//            CancellationToken cancellationToken = default)
//        {
//            var user = new UserAuthentication
//            {
//                Name = name,
//                Email = email,
//                MobileNumber = mobileNumber,
//                Password = PasswordHelper.HashPassword(password),
//                Role = "User"
//            };

//            await _userRepository.AddAsync(user, cancellationToken);
//            await _userRepository.SaveChangesAsync(cancellationToken);

//            return user.Id;
//        }

//        public Task<UserAuthentication?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
//            => _userRepository.GetByEmailAsync(email, cancellationToken);

//        public Task<UserAuthentication?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
//            => _userRepository.GetByIdAsync(id, cancellationToken);

//        public Task<List<UserAuthentication>> GetAllUsersAsync(CancellationToken cancellationToken = default)
//            => _userRepository.GetAllAsync(cancellationToken);

//        public async Task AddUserAsync(UserAuthentication user, CancellationToken cancellationToken = default)
//        {
//            user.Password = PasswordHelper.HashPassword(user.Password);
//            await _userRepository.AddAsync(user, cancellationToken);
//            await _userRepository.SaveChangesAsync(cancellationToken);
//        }

//        public Task UpdateUserAsync(UserAuthentication user, CancellationToken cancellationToken = default)
//            => _userRepository.UpdateAsync(user, cancellationToken);

//        public Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
//            => _userRepository.DeleteAsync(id, cancellationToken);

//        public Task<int> CountUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
//            => _userRepository.CountByRoleAsync(role, cancellationToken);
//    }
//}




using PackageFoodManagementSystem.Services.Helpers;

using PackageFoodManagementSystem.Repository.Models;

using System.Collections.Generic;

using System.Threading;

using System.Threading.Tasks;

using PackageFoodManagementSystem.Services.Interfaces;

using PackageFoodManagementSystem.Repository.Interfaces;
using System.Linq; // Added for list operations

using System.Linq; // Added for list operations

namespace PackageFoodManagementSystem.Services.Implementations

{

    public class UserService : IUserService

    {

        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;


        public UserService(IUserRepository userRepository, ICustomerRepository customerRepository)

        {

            _userRepository = userRepository;
            _customerRepository = customerRepository;

            _customerRepository = customerRepository;

        }

        public async Task<int> CreateUserAsync(
     string name,
     string mobileNumber,
     string email,
     string password,
     CancellationToken cancellationToken = default)
        {
            // 1. Create the Auth record
            var user = new UserAuthentication
            {
                Name = name,
                Email = email,
                MobileNumber = mobileNumber,
                Password = PasswordHelper.HashPassword(password),
                Role = "User"
            };

            // 2. Add User and Save immediately to get the generated User.Id
            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            var customer = new Customer
            {
                Name = name,
                Email = email,
                Phone = mobileNumber,
                Status = "Active",
                UserId = user.Id
            };

            await _customerRepository.AddAsync(customer);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // 3. Create the Customer record linked to that ID
            

            // 5. CRITICAL STEP: 
            // If _customerRepository uses a different context, you MUST call Save on it.
            // If they share the same context, this call will push the customer to the DB.

            return user.Id;
        }

        // --- UPDATED DELETE METHOD TO PREVENT SQL ERROR ---
        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            // 1. Find the customer linked to this UserId
            var allCustomers = await _customerRepository.GetAllAsync();
            var customer = allCustomers.FirstOrDefault(c => c.UserId == id);

            // 2. Delete Customer first to satisfy Foreign Key constraint
            if (customer != null)
            {
                await _customerRepository.DeleteAsync(customer.CustomerId);
            }

            // 3. Now delete the Authentication record
            await _userRepository.DeleteAsync(id, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        // --- UPDATED DELETE METHOD TO PREVENT SQL ERROR ---


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

        public Task<int> CountUsersByRoleAsync(string role, CancellationToken cancellationToken = default)

            => _userRepository.CountByRoleAsync(role, cancellationToken);

    }
}


