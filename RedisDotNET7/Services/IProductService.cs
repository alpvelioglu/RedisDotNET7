using RedisDotNET7.Models;

namespace RedisDotNET7.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);

        // ProductDTO dönmek lazımmış
    }
}
