using Microsoft.EntityFrameworkCore;

namespace RedisCacheApp.API.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.0M },
                new Product { Id = 2, Name = "Product 2", Price = 20.0M },
                new Product { Id = 3, Name = "Product 3", Price = 30.0M }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
