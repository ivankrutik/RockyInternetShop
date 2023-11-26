using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RockyDataAccess.Data;
using RockyModels;
using RockyUtility;

namespace RockyDataAccess.Reporitory.ProductDomain
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public IEnumerable<SelectListItem>? GetAllDropDownList(string obj)
        {
            if (obj == WebConstant.CategoryName)
            {
                return _appDbContext.Category.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             });
            }
            if (obj == WebConstant.AppTypeName)
            {
                return _appDbContext.ApplicationType.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            _appDbContext.Product.Update(product);
        }
    }
}
