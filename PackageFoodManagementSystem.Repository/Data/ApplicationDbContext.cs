
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<UserAuthentication> UserAuthentications { get; set; }
        public DbSet<Batch> Batch { get; set; }
    }
}
