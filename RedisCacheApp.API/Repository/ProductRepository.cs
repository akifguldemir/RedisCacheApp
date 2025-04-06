using Microsoft.EntityFrameworkCore;
using RedisCacheApp.API.Models;

namespace RedisCacheApp.API.Repository
{
    public class ProductRepository : IProductRepository
    {
       private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return  await _context.Products.FindAsync(id);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
