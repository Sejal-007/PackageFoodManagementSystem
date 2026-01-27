using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Implementations
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerAddressRepository _repository;

        // COMBINED CONSTRUCTOR: This is the correct way to inject multiple dependencies
        public CustomerAddressService(ApplicationDbContext context, ICustomerAddressRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<CustomerAddress>> GetAllAsync()
        {
            // Fetching directly from context ensures we see the latest DB changes
            return await _context.CustomerAddresses.ToListAsync();
        }

        public async Task<CustomerAddress> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(CustomerAddress address)
        {
            // Ensure the address is added to the Change Tracker
            await _context.CustomerAddresses.AddAsync(address);

            // CRITICAL: This saves the data to the physical SQL database
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerAddress address)
        {
            await _repository.UpdateAsync(address);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerAddress>> GetAddressesByUserIdAsync(int userId)
        {
            // Fixed the NotImplementedException - now it actually filters!
            return await _context.CustomerAddresses
                                 .Where(a => a.CustomerId == userId)
                                 .ToListAsync();
        }

        Task<dynamic> ICustomerAddressService.GetAddressesByUserIdAsync(int value)
        {
            throw new NotImplementedException();
        }
    }
}