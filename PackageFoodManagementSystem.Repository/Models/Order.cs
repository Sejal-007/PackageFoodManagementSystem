using PackageFoodManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int OrderID { get; internal set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int CreatedByUserID { get; set; }

        [Required]
        [MaxLength(30)]
        public required string OrderStatus { get; set; } // Added required

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string? DeliveryAddress { get; set; } // Kept nullable as per your code

        public DateTime LastUpdateOn { get; set; }

        // Initialized as a concrete List to avoid "cannot create instance of interface" error
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Navigation Property: Marked as nullable
        public virtual Bill? Bill { get; set; }
    }
}