using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Data.SqlTypes;

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

        var cart = _context.Carts

            .Include(c => c.CartItems)

            .FirstOrDefault(c => c.UserAuthenticationId == userAuthId && c.IsActive);

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

            _context.SaveChanges(); // 🔥 CartId GENERATED HERE

        }

        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)

        {

            item = new CartItem

            {

                CartId = cart.CartId, // ✅ NOW SAFE

                ProductId = productId,

                Quantity = 1

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
        // 1. Assign the result to the 'cart' variable
        var cart = _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c =>
                c.UserAuthenticationId == userAuthId &&
                c.IsActive);

        // 2. Check if it's null and create if necessary
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userAuthId,
                UserAuthenticationId = userAuthId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        // 3. Return the cart at the END of the method
        return cart;
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

    

    string? ICartService.GetCartByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    void ICartService.AddToCart(int userAuthId, int productId)
    {
        throw new NotImplementedException();
    }
}