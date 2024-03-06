using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;

namespace CoffeeShop.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        public Product? GetProductDetail(int id)
        {
            return _context.Products.FirstOrDefault(p=>p.Id == id);
        }

        public IEnumerable<Product> GetTrendingProducts()
        {
            return _context.Products.Where(p=>p.IsTrendingProduct);
        }
    }
}
