using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RockyDataAccess.Data;
using RockyModels;
using RockyUtility;

namespace RockyDataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(WebConstant.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebConstant.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebConstant.CustomerRole)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new AppUser() { UserName = "admin", Email = "admin@gmail.com", EmailConfirmed = true, FullName = "Admin", PhoneNumber = "+79090001122" }, "admin123*").GetAwaiter().GetResult();
                var user = _db.Users.FirstOrDefault(x => x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, WebConstant.AdminRole).GetAwaiter().GetResult();
            }
        }
    }
}
