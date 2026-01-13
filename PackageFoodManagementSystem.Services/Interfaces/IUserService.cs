using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
