using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        //[Required]
        public int? UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public virtual UserAuthentication User { get; set; }

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