using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _context;
        public string? ShoppingCartId { get; set; }
        public ShoppingCartRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            AppDbContext context = services.GetService<AppDbContext>() ??
                throw new Exception("Error initializing DB context");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }


        public void AddToCart(Product product)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Qty = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Qty++;
            }
           _context.SaveChanges();
        }


        public void ClearCart()
        {
            var cartItems = _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId);
            _context.ShoppingCartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId)
                 .Include(p => p.Product).ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            var totalCost = _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId)
                  .Select(s => s.Product.Price * s.Qty).Sum();
            return totalCost;
        }

        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);
            var quantity = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Qty > 1)
                {
                    shoppingCartItem.Qty--;
                    quantity = shoppingCartItem.Qty;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _context.SaveChanges();
            return quantity;
        }
    }
}
