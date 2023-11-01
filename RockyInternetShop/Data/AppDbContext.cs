using Microsoft.EntityFrameworkCore;
using RockyInternetShop.Models;

namespace RockyInternetShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }
    }
}
