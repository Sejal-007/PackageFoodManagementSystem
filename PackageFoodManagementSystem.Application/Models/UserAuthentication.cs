namespace PackageFoodManagementSystem.Application.Models
{
    public class UserAuthentication
    {
        public int Id { get; set; } // Primary Key
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // e.g., "Admin", "User"
    }
}