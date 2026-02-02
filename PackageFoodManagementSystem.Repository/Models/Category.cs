using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageFoodManagementSystem.Repository.Models
{
    [Table("Categories")] // Forces EF to use the "Categories" table name
    public class Category
    {
        [Key] // Explicitly defines the PK to stop EF from looking for other IDs
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}