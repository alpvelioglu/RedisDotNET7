using RedisDotNET7.Models;
using RedisDotNET7.Repository;

namespace RedisDotNET7.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await _productRepo.CreateAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepo.GetAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            // mapper dto
            return await _productRepo.GetByIdAsync(id);
        }
    }
}
