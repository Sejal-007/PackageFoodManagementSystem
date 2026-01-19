using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public required string ProductName { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
<<<<<<< HEAD
=======
<<<<<<< HEAD

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Status { get; set; }
=======
>>>>>>> cea734f18942ffda35f30bb5396d6b26c0f611c6
        // <-- Add this line right here
        public int Quantity { get; set; }
        public required string Category { get; set; }
        public bool IsActive { get; set; } = true;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 3a07ba1 (addedbackendto prd model)
=======
>>>>>>> 3a07ba1 (addedbackendto prd model)
>>>>>>> cea734f18942ffda35f30bb5396d6b26c0f611c6

    }
}