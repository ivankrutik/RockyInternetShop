using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RockyModels;

namespace RockyDataAccess.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=DESKTOP-KQEGRRC;Database=RockyShop;Trusted_Connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
            optionsBuilder.LogTo(System.Console.WriteLine);
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
