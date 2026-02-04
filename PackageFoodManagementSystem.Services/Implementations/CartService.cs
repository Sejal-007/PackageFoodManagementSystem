using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddItem(int userAuthId, int productId)
    {
        var cart = GetOrCreateCart(userAuthId);
        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            item = new CartItem { CartId = cart.CartId, ProductId = productId, Quantity = 1 };
            _context.CartItems.Add(item);
        }
        else
        {
            item.Quantity++;
        }
        _context.SaveChanges();
    }

    public void DecreaseItem(int userAuthId, int productId)
    {
        var cart = GetOrCreateCart(userAuthId);
        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (item == null) return;

        item.Quantity--;
        if (item.Quantity <= 0) _context.CartItems.Remove(item);
        _context.SaveChanges();
    }

    public void Remove(int userAuthId, int productId)
    {
        var cart = GetActiveCart(userAuthId);
        if (cart == null) return;
        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }

    public Cart GetActiveCart(int userAuthId)
    {
        return _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.UserAuthenticationId == userAuthId && c.IsActive);
    }

    public async Task<Cart> GetActiveCartAsync(int userAuthId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserAuthenticationId == userAuthId && c.IsActive);
    }

    private Cart GetOrCreateCart(int userAuthId)
    {
        var cart = GetActiveCart(userAuthId);
        if (cart == null)
        {
            cart = new Cart
            {
                UserAuthenticationId = userAuthId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        return cart;
    }

    public string? GetCartByUserId(int userId) => null;
    public void AddToCart(int userAuthId, int productId) => AddItem(userAuthId, productId);
}