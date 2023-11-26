using Microsoft.AspNetCore.Mvc.Rendering;
using RockyModels;

namespace RockyDataAccess.Reporitory.ProductDomain
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<SelectListItem>? GetAllDropDownList(string obj);
    }
}
