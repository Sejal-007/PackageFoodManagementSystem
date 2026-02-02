using PackageFoodManagementSystem.Repository.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Unified Namespace
namespace PackageFoodManagementSystem.Repository.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]

        public int OrderID { get; set; }

        public int BillID { get; set; }

        [Required]
        [MaxLength(30)]
        public required string PaymentMethod { get; set; }  // Cash, UPI, Card

        [Required]
        [MaxLength(20)]
        public required string PaymentStatus { get; set; }  // Pending, Success, Failed

        [Required]
        public required string TransactionReference { get; set; }

        public DateTime PaymentDate { get; set; }

        [ForeignKey("OrderID")]

        public Order Order { get; set; }

        public decimal AmountPaid { get; set; }

        public string? GatewayResponse { get; set; }

        // Navigation Property: Marked as nullable to satisfy compiler
        [ForeignKey("BillID")]
        public virtual Bill? Bill { get; set; }
    }
}