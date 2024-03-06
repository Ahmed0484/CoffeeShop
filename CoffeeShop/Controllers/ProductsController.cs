using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Shop()
        {
            return View(_repo.GetAllProducts());
        }

        public IActionResult Details(int id)
        {
            var p = _repo.GetProductDetail(id);
            if(p==null) return NotFound();
            return View(p);
        }
    }
}
    
