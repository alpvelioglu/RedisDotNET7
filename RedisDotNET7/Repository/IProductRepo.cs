using RedisDotNET7.Models;

namespace RedisDotNET7.Repository
{
    public interface IProductRepo
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
