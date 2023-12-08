using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RockyModels;
using RockyModels.InquiryDomain;
using RockyModels.OrderDomain;

namespace RockyDataAccess.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(System.Console.WriteLine);
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<InquiryDetail> InquiryDetail { get; set; }
        public DbSet<InquiryHeader> InquiryHeader { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
    }
}
