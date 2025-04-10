using RedisCacheApp.API.Models;

namespace RedisCacheApp.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
