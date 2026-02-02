using PackageFoodManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface ICartService
    {
        // Use 'userAuthId' to match your CartService.cs exactly

        void AddToCart(int userAuthId, int productId);

        void AddItem(int userAuthId, int productId);

        void DecreaseItem(int userAuthId, int productId);

        void Remove(int userAuthId, int productId);

        Cart GetActiveCart(int userAuthId);
        string? GetCartByUserId(int userId);
    }
}