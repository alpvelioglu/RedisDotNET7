using Microsoft.EntityFrameworkCore;
using RedisDotNET7.Models;

namespace RedisDotNET7.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDBContext _context;

        public ProductRepo(AppDBContext context)
        {
           _context = context;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
