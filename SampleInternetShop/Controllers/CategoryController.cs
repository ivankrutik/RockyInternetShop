using Microsoft.AspNetCore.Mvc;

namespace SampleInternetShop.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
