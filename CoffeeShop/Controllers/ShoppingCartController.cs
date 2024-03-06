using CoffeeShop.Models.Interfaces;
using CoffeeShop.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shopRepo;
        private readonly IProductRepository _productRepo;

        public ShoppingCartController(IShoppingCartRepository shopRepo,
            IProductRepository productRepo)
        {
            _shopRepo = shopRepo;
            _productRepo = productRepo;
        }
        public IActionResult Index()
        {
            var items = _shopRepo.GetShoppingCartItems();//fn
            _shopRepo.ShoppingCartItems = items;//prop
            ViewBag.CartTotal = _shopRepo.GetShoppingCartTotal();
            return View(items);
        }
        public RedirectToActionResult AddToShoppingCart(int pId)
        {
            var product = _productRepo.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                _shopRepo.AddToCart(product);
                int cartCount = _shopRepo.GetShoppingCartItems().Count;
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pId)
        {
            var product = _productRepo.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                _shopRepo.RemoveFromCart(product);
                int cartCount = _shopRepo.GetShoppingCartItems().Count;
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index");
        }
    }
}
