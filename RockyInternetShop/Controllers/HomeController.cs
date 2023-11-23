using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RockyDataAccess.Data;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;
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

        public IActionResult Details(long id)
        {
            bool IsExistInCart = false;
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                var shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).ToList();
                if (shoppingCarts.Exists(z => z.ProductId == id))
                {
                    IsExistInCart = true;
                }
            }

            DetailsVM vm = new DetailsVM()
            {
                Product = _appDbContext.Product.Include(x => x.Category).Include(z => z.AppType).FirstOrDefault(z => z.Id == id),
                IsExistInCart = IsExistInCart
            };

            return View(vm);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(long id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).ToList();
            }
            shoppingCarts.Add(new ShoppingCart() { ProductId = id });
            HttpContext.Session.Set(WebConstant.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(long id)
        {
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                var shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).ToList();
                shoppingCarts.Remove(shoppingCarts.FirstOrDefault(z => z.ProductId == id));
                HttpContext.Session.Set(WebConstant.SessionCart, shoppingCarts);
            }
            return RedirectToAction(nameof(Index));
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