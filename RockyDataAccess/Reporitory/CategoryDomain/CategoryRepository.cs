using RockyDataAccess.Data;
using RockyModels;

namespace RockyDataAccess.Reporitory.CategoryDomain
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public void Update(Category category)
        {
            var objDb = _appDbContext.Category.FirstOrDefault(x => x.Id == category.Id);

            if (objDb != null)
            {
                objDb.Name = category.Name;
                objDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
