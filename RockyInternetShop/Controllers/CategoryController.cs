using Microsoft.AspNetCore.Mvc;
using SampleInternetShop.Data;
using SampleInternetShop.Models;

namespace SampleInternetShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _dbContext.Category;
            return View(categories);
        }
    }
}
