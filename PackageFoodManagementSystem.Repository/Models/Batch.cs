using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    [Table("Batch")] // Maps to the singular 'Batch' table in your SQL screenshot
    public class Batch
    {


        [Key]
        public int BatchId { get; set; }
        public int ProductId { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }

        // This must match the column name exactly as seen in your SQL Results
        public int CategoryId { get; set; }

        // Navigation Property for Entity Framework

        [NotMapped]
        public List<Category>? Categories { get; set; }

        [NotMapped]
        public List<Product>? Products { get; set; }
    }
}