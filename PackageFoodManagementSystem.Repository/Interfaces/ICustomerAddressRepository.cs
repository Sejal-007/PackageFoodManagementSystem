using PackageFoodManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface ICustomerAddressRepository
    {

        Task<IEnumerable<CustomerAddress>> GetAllAsync();
        Task<CustomerAddress> GetByIdAsync(int id);
        Task AddAsync(CustomerAddress address);
        Task UpdateAsync(CustomerAddress address);
        Task DeleteAsync(int id);

    }
}
