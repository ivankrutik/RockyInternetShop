using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RockyDataAccess.Reporitory.CategoryDomain;
using RockyDataAccess.Reporitory.ProductDomain;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;
using System.Diagnostics;
using System.Linq;

namespace RockyInternetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _prodRep;
        private readonly ICategoryRepository _catRep;

        public HomeController(ILogger<HomeController> logger, IProductRepository prodRep, ICategoryRepository catRep)
        {
            _logger = logger;
            _prodRep = prodRep;
            _catRep = catRep;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _prodRep.GetAll(includedProperties: "Category,AppType"),
                Categories = _catRep.GetAll()
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
                Product = _prodRep.FirstOrDefault(filter: z => z.Id == id, includedProperties: "Category,AppType"),
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