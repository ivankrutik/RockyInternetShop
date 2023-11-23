using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Data;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;
using System.Security.Claims;
using System.Text;

namespace RockyInternetShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM ProdUserVm { get; set; }

        public CartController(AppDbContext appDbContext, IWebHostEnvironment webHostEnv, IEmailSender emailSender)
        {
            _appDbContext = appDbContext;
            _webHostEnv = webHostEnv;
            _emailSender = emailSender;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPostAsync(ProductUserVM ProdUserVm)
        {
            var PathToTemplate = _webHostEnv.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";
            var Subject = "New Inquiry";
            string HtmlBody = string.Empty;
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var prod in ProdUserVm.Products)
            {
                stringBuilder.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br></br>");
            }

            string messageBody = string.Format(HtmlBody, ProdUserVm.AppUser.FullName, ProdUserVm.AppUser.Email, ProdUserVm.AppUser.PhoneNumber, stringBuilder.ToString());

            await _emailSender.SendEmailAsync(WebConstant.EmailAdmin, Subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
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
