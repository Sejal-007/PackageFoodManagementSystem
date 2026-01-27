using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class OrderStatusHistory
    {
        [Key]
        public int StatusHistoryID { get; set; }
        public int OrderID { get; set; }
        public string Status { get; set; }
        public DateTime ChangedOn { get; set; }
        public int ChangedBy { get; set; }
        public string? Remarks { get; set; }

        public Order Order { get; set; }
    }
}
