namespace PackageFoodManagementSystem.Repository.Models
{
    public class Customer
    {
        public int Id { get; set; } // <--- Add this line
        public string Name { get; set; }
        public string Email { get; set; }
        // ... other properties
    }
}