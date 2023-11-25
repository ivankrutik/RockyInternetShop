using RockyDataAccess.Data;
using RockyModels;

namespace RockyDataAccess.Reporitory.AppTypeDomain
{
    public class AppTypeRepository : Repository<ApplicationType>, IAppTypeRepository
    {
        private readonly AppDbContext _appDbContext;

        public AppTypeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public void Update(ApplicationType type)
        {
            var objDb = _appDbContext.Category.FirstOrDefault(x => x.Id == type.Id);

            if (objDb != null)
            {
                objDb.Name = type.Name;
            }
        }
    }
}
