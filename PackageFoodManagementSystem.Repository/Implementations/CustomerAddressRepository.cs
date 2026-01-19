using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Repository.Implementations
{
    public class CustomerAddressRepository : ICustomerAddressRepository
    {


        private readonly ApplicationDbContext _context;

        public CustomerAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerAddress>> GetAllAsync()
        {
            return await _context.CustomerAddresses
                .Include(a => a.Customer)
                .ToListAsync();
        }

        public async Task<CustomerAddress?> GetByIdAsync(int id)
        {
            return await _context.CustomerAddresses
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AddressId == id);
        }

        public async Task AddAsync(CustomerAddress address)
        {
            _context.CustomerAddresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerAddress address)
        {
            _context.CustomerAddresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _context.CustomerAddresses.FindAsync(id);
            if (address != null)
            {
                _context.CustomerAddresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}
