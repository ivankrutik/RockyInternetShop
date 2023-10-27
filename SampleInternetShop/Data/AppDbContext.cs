using Microsoft.EntityFrameworkCore;
using SampleInternetShop.Models;

namespace SampleInternetShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
    }
}
