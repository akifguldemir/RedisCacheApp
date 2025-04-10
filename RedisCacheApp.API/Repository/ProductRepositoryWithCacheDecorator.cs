using RedisCacheApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisCacheApp.API.Repository
{ 
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private const string productKey = "productCaches";
        private readonly IDatabase _cacheRepository;


        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDatabase(0);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey)) {
                await _cacheRepository.HashSetAsync(productKey, newProduct.Id.ToString(), JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if(!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }

            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);

            foreach (var cacheProduct in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(cacheProduct.Value);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            if(await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id.ToString());
                return product.HasValue ?  JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();

            return products.FirstOrDefault(p => p.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(product =>
            {
                _cacheRepository.HashSetAsync(productKey, product.Id.ToString(), JsonSerializer.Serialize(product));
            });
            
            return products;
        }
    }
}
