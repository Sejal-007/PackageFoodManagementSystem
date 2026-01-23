using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ================= ADD / INCREASE =================
    public void AddItem(int userAuthId, int productId)
    {
        var cart = GetOrCreateCart(userAuthId);

        // ✅ CartItems is GUARANTEED not null
        var item = cart.CartItems
      .FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            item = new CartItem
            {
                ProductId = productId,
                Quantity = 1,
                CartId = cart.CartId   // 🔴 REQUIRED for FK
            };

            _context.CartItems.Add(item);
        }
        else
        {
            item.Quantity++;
        }

        _context.SaveChanges();
    }

    // ================= DECREASE =================
    public void DecreaseItem(int userAuthId, int productId)
    {
        var cart = GetOrCreateCart(userAuthId);

        var item = cart.CartItems
          .FirstOrDefault(x => x.ProductId == productId);

        if (item == null) return;

        item.Quantity--;

        if (item.Quantity <= 0)
        {
            _context.CartItems.Remove(item);
        }

        _context.SaveChanges();
    }

    // ================= REMOVE =================
    public void Remove(int userAuthId, int productId)
    {
        var cart = GetActiveCart(userAuthId);
        if (cart == null) return;

        var item = cart.CartItems
          .FirstOrDefault(x => x.ProductId == productId);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }

    // ================= GET ACTIVE CART =================
    public Cart GetActiveCart(int userAuthId)
    {
        return _context.Carts
          .Include(c => c.CartItems)               // 🔴 VERY IMPORTANT
                .ThenInclude(ci => ci.Product)
          .FirstOrDefault(c =>
            c.UserAuthenticationId == userAuthId &&
            c.IsActive);
    }

    // ================= GET OR CREATE CART =================
    private Cart GetOrCreateCart(int userAuthId)
    {
        var cart = _context.Carts
          .Include(c => c.CartItems)               // 🔴 FIX FOR NULL ERROR
                .FirstOrDefault(c =>
            c.UserAuthenticationId == userAuthId &&
            c.IsActive);

        if (cart == null)
        {
            cart = new Cart
            {
                UserAuthenticationId = userAuthId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()     // 🔴 NEVER NULL
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        else if (cart.CartItems == null)
        {
            // 🔴 SAFETY NET (VERY IMPORTANT)
            cart.CartItems = new List<CartItem>();
        }

        return cart;
    }

    void ICartService.AddToCart(int userId, int productId)
    {
        throw new NotImplementedException();
    }
}