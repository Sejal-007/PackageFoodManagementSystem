using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary Key
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; } // Foreign Key for Customer
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
    }
}