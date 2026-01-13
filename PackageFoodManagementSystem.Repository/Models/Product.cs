using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column (TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Status { get; set; }

    }
}