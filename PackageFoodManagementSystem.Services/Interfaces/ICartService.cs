using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface ICartService
    {
        void AddItem(int userAuthId, int productId);
        void DecreaseItem(int userAuthId, int productId);
        void Remove(int userAuthId, int productId);
        Cart GetActiveCart(int userAuthId);
        Task<Cart> GetActiveCartAsync(int userAuthId);
        string? GetCartByUserId(int userId);
        void AddToCart(int userAuthId, int productId);
    }
}