using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IShoppingCartRepository _repo;

        public OrderRepository(AppDbContext context,
            IShoppingCartRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        public void PlaceOrder(Order order)
        {
            var shoppingCartItems = _repo.GetShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Qty,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price
                };
                order.OrderDetails.Add(orderDetail);
            }
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _repo.GetShoppingCartTotal();
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
