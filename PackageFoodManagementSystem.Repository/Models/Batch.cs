using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Batch
    {
        [Key]
        public int BatchId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime ManufactureDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation Property for Entity Framework
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}

