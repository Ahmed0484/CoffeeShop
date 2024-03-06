using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using CoffeeShop.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IShoppingCartRepository _shopRepo;

        public OrdersController(IOrderRepository orderRepo,
            IShoppingCartRepository shopRepo)
        {
            _orderRepo = orderRepo;
            _shopRepo = shopRepo;
        }
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            _orderRepo.PlaceOrder(order);
            _shopRepo.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);
            return RedirectToAction("CheckoutComplete");
        }

        public IActionResult CheckoutComplete()
        {
            return View();
        }

    }
}
