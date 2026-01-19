using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PackageFoodManagementSystem.Repository.Models
{
    public class CustomerAddress
    {
        [Key]
        public int AddressId { get; set; }

        public int CustomerId { get; set; } // Foreign Key

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public Customer Customer { get; set; } // Navigation property


    }
}
