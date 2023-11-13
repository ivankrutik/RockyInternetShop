using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyInternetShop.Data;
using RockyInternetShop.Models;
using RockyInternetShop.Models.ViewModel;
using RockyInternetShop.Utility;
using System.Security.Claims;

namespace RockyInternetShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _appDbContext;

        [BindProperty]
        public ProductUserVM ProdUserVm { get; set; }

        public CartController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var shpCarts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null &&
                HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                shpCarts = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }
            var prodId = shpCarts.Select(z => z.ProductId);
            var products = _appDbContext.Product.Where(z => prodId.Contains(z.Id));

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var UserId = User.FindFirstValue(ClaimTypes.Name);

            var shpCarts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null &&
                HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                shpCarts = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }
            var prodId = shpCarts.Select(z => z.ProductId);
            var products = _appDbContext.Product.Where(z => prodId.Contains(z.Id)).ToList();

            ProdUserVm = new ProductUserVM
            {
                AppUser = UserId != null ? _appDbContext.AppUsers.FirstOrDefault(z => z.UserName == UserId) : new AppUser(),
                Products = products
            };

            return View(ProdUserVm);
        }

        public IActionResult Remove(long id)
        {
            var shpCarts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null &&
                HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                shpCarts = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
                var obj = shpCarts.FirstOrDefault(z => z.ProductId == id);
                if (obj != null)
                {
                    shpCarts.Remove(obj);
                    HttpContext.Session.Set<List<ShoppingCart>>(WebConstant.SessionCart, shpCarts);
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
