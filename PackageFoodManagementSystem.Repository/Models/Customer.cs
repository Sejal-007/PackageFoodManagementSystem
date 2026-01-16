using System.ComponentModel.DataAnnotations;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string Phone { get; set; }

        public string? Status { get; set; }

        public ICollection<CustomerAddress> Addresses { get; set; }
    }
}