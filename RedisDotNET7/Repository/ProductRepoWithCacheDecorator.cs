using RedisDotNET7.Models;
using RedisDotNET7.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisDotNET7.Repository
{
    public class ProductRepoWithCacheDecorator : IProductRepo
    {
        private readonly IProductRepo _productRepo;
        private readonly RedisService _redisService;
        private const string productKey = "productCachhes";
        private readonly IDatabase _cacheRepo;
        public ProductRepoWithCacheDecorator(IProductRepo productRepo, RedisService redisService)
        {
            _productRepo = productRepo;
            _redisService = redisService;
            _cacheRepo = _redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepo.CreateAsync(product);
            if(await _cacheRepo.KeyExistsAsync(productKey))
            {
                await _cacheRepo.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if(!await _cacheRepo.KeyExistsAsync(productKey))
            {
                return await LoadCacheFromDbAsync();
            }
            var products = new List<Product>();
            var cacheProducts = await _cacheRepo.HashGetAllAsync(productKey);
            foreach(var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepo.KeyExists(productKey))
            {
                var product = await _cacheRepo.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        } 

        private async Task<List<Product>> LoadCacheFromDbAsync()
        {
            var products = await _productRepo.GetAsync();
            products.ForEach(p =>
            {
                _cacheRepo.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }
    }
}
