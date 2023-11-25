using RockyModels;

namespace RockyDataAccess.Reporitory.AppTypeDomain
{
    public interface IAppTypeRepository : IRepository<ApplicationType>
    {
        void Update(ApplicationType type);
    }
}
