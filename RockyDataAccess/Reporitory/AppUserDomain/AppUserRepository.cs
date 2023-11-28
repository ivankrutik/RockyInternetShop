using Microsoft.EntityFrameworkCore;
using RockyDataAccess.Data;
using RockyModels;

namespace RockyDataAccess.Reporitory.AppUserDomain
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public AppUserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
