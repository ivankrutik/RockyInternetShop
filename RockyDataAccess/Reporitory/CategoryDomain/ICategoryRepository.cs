using RockyModels;

namespace RockyDataAccess.Reporitory.CategoryDomain
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
