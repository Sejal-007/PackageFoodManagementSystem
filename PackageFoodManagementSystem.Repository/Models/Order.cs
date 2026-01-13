using System;
using System.Collections.Generic;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary Key
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; } // Foreign Key for Customer
        public decimal TotalAmount { get; set; }
    }
}