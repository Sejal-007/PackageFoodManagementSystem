

namespace PackageFoodManagementSystem.Repository.Models
{
    public class Wallet
    {
        public int WalletId { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key to link to your User
        public decimal Balance { get; set; }

        public virtual UserAuthentication User { get; set; }
    }
}