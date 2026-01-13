using PackageFoodManagementSystem.Repository.Models;
using System;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models

{

    public class Bill

    {

        [Key]

        public int BillID { get; set; }

        // Foreign Key to Orders

        [Required]

        public int OrderID { get; set; }

        [Required]

        public DateTime BillDate { get; set; }

        [Required]

        public decimal SubtotalAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        [Required]

        public decimal FinalAmount { get; set; }

        [Required]

        [MaxLength(20)]

        public string? BillingStatus { get; set; }   // Generated, Paid, Cancelled

        // Navigation Properties

        [ForeignKey("OrderID")]

        public Order Order { get; set; }

        public Payment Payment { get; set; }

    }

}
