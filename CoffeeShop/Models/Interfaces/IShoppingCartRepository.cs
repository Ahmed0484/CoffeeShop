namespace CoffeeShop.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        void AddToCart(Product product);
        int RemoveFromCart(Product product);
        List<ShoppingCartItem> GetShoppingCartItems();
        decimal GetShoppingCartTotal();
        void ClearCart();
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
    }
}
