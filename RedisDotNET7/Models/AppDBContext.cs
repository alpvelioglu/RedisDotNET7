using Microsoft.EntityFrameworkCore;

namespace RedisDotNET7.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
              
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem 1", Price = 100 },
                new Product() { Id = 2, Name = "Kalem 2", Price = 150 },
                new Product() { Id = 3, Name = "Kalem3 ", Price = 200 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
