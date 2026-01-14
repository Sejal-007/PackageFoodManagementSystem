using PackageFoodManagementSystem.Repository.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Batch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string BatchNumber { get; set; }
        public DateTime ProductionDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public string? Description { get; set; }
        // Module: Quantity Tracking
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }

        // Module: Status Management (Active, Expired, Recalled)
        public BatchStatus Status { get; set; } 

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }

    public enum BatchStatus
    {
        Active,
        NearExpiry,
        Expired,
        Recalled,
        OutOfStock
    }
}