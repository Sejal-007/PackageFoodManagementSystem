using PackageFoodManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface ICustomerRepository
    {

        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
