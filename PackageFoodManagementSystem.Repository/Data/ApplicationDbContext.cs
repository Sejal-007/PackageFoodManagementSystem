using Microsoft.EntityFrameworkCore;

using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Data
{
    public class ApplicationDbContext : DbContext

    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)

            : base(options)

        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<UserAuthentication> UserAuthentications { get; set; }
        public DbSet<Batch> Batches { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Force EF to use the singular name 'Customer' to match your SQL Server
            modelBuilder.Entity<Customer>().ToTable("Customer");

            // Optional: If 'UserAuthentications' also gives an error, 
            // you can force its name here too:
            // modelBuilder.Entity<UserAuthentication>().ToTable("UserAuthentications");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This tells EF to ignore the "Pending Changes" warning and just run the update
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

    }

}
