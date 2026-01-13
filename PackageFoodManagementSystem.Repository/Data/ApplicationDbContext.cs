using Microsoft.EntityFrameworkCore;

using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Data {

    public class ApplicationDbContext : DbContext

    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)

            : base(options)

        {

        }

        public DbSet<PackageFoodManagementSystem.Repository.Models.Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<UserAuthentication> UserAuthentication { get; set; }

        public DbSet<Batch> Batch { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Order>(entity =>
        //    {
        //        entity.HasKey(e => e.ProductId);
        //        entity.Property(e => e.ProductName).IsRequired();
        //        entity.Property(e => e.IsActive).HasDefaultValue(true);
        //        entity.Property(e => e.Quantity).HasDefaultValue(0);
        //    });
        //}

    }

}
