using System.Collections.Generic;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Application.ViewModels
{
    public class BatchCreateViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        // Add other properties as needed for batch creation, e.g.:
        // public Batch Batch { get; set; }
    }
}