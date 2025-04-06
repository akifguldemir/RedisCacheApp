using RedisCacheApp.API.Models;

namespace RedisCacheApp.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateAsync(Product product);

    }
}
