namespace PackageFoodManagementSystem.Repository.Models
{
    public class Batch
    {
        public int Id { get; set; } // EF will automatically pick this as the Primary Key

        public string BatchNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        // ... other properties
    }
}