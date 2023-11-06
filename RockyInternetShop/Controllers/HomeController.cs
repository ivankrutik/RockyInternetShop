using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RockyInternetShop.Data;
using RockyInternetShop.Models;
using RockyInternetShop.Models.ViewModel;
using System.Diagnostics;

namespace RockyInternetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _appDbContext.Product.Include(c => c.Category).Include(z => z.AppType),
                Categories = _appDbContext.Category
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}