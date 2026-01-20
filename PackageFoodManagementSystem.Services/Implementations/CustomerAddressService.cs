using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Services.Implementations
{
    public class CustomerAddressService : ICustomerAddressService // Add the interface here
    {
        private readonly ICustomerAddressRepository _repository;
        public CustomerAddressService(ICustomerAddressRepository repository) => _repository = repository;

        public Task<IEnumerable<CustomerAddress>> GetAllAsync() => _repository.GetAllAsync();
        public Task<CustomerAddress> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task AddAsync(CustomerAddress address) => _repository.AddAsync(address);
        public Task UpdateAsync(CustomerAddress address) => _repository.UpdateAsync(address);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

        public Task<dynamic> GetAddressesByUserIdAsync(int value)
        {
            throw new NotImplementedException();
        }
    }
}
