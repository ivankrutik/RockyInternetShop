using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RockyDataAccess.Reporitory.AppUserDomain;
using RockyDataAccess.Reporitory.InquiryDomain;
using RockyDataAccess.Reporitory.OrderDomain;
using RockyDataAccess.Reporitory.ProductDomain;
using RockyModels;
using RockyModels.InquiryDomain;
using RockyModels.OrderDomain;
using RockyModels.ViewModel;
using RockyUtility;
using System.Security.Claims;
using System.Text;

namespace RockyInternetShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly IEmailSender _emailSender;
        private readonly IProductRepository _prodRep;
        private readonly IAppUserRepository _userRep;
        private readonly IInquiryHeaderRepository _inqHdrRep;
        private readonly IInquiryDetailRepository _inqDtlRep;
        private readonly IOrderHeaderRepository _ordHdrRep;
        private readonly IOrderDetailRepository _ordDtlRep;

        [BindProperty]
        public ProductUserVM ProdUserVm { get; set; }

        public CartController(IWebHostEnvironment webHostEnv,
                                IEmailSender emailSender,
                                IProductRepository prodRep,
                                 IAppUserRepository userRep,
                                 IInquiryHeaderRepository inqHdrRep,
                                 IInquiryDetailRepository inqDtlRep,
                                 IOrderHeaderRepository ordHdrRep,
                                 IOrderDetailRepository ordDtlRep
            )
        {
            _webHostEnv = webHostEnv;
            _emailSender = emailSender;
            _prodRep = prodRep;
            _userRep = userRep;
            _inqHdrRep = inqHdrRep;
            _inqDtlRep = inqDtlRep;
            _ordHdrRep = ordHdrRep;
            _ordDtlRep = ordDtlRep;
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
            var products = _prodRep.GetAll(filter: z => prodId.Contains(z.Id)).ToList();

            foreach (var cart in shpCarts)
            {
                products.FirstOrDefault(x => x.Id == cart.ProductId).QuantityTemp = (int)cart.QuantityTemp;
            }

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            foreach (var product in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = product.Id, QuantityTemp = product.QuantityTemp });
            }
            HttpContext.Session.Set<List<ShoppingCart>>(WebConstant.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            AppUser appUser = new AppUser();

            if (User.IsInRole(WebConstant.AdminRole))
            {
                if (HttpContext.Session.Get(WebConstant.SessionInquiryId) != null)
                {
                    var head = _inqHdrRep.FirstOrDefault(filter: x => x.Id == HttpContext.Session.Get<long>(WebConstant.SessionInquiryId));
                    if (head != null)
                    {
                        appUser = new AppUser
                        {
                            Email = head.Email,
                            FullName = head.FullName,
                            PhoneNumber = head.Phone
                        };
                    }
                }
                else
                {
                    appUser = new AppUser();
                }
            }
            else
            {
                //var UserId = User.FindFirstValue(ClaimTypes.Name);
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                appUser = _userRep.FirstOrDefault(x => x.Id == claim.Value);
            }

            var shpCarts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null &&
                HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Any())
            {
                shpCarts = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }
            var prodId = shpCarts.Select(z => z.ProductId);
            var products = _prodRep.GetAll(filter: z => prodId.Contains(z.Id)).ToList();
            foreach (var product in products)
            {
                product.QuantityTemp = shpCarts.FirstOrDefault(z => z.ProductId == product.Id).QuantityTemp;
            }

            ProdUserVm = new ProductUserVM
            {
                AppUser = appUser,
                Products = products
            };

            return View(ProdUserVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPostAsync(ProductUserVM ProdUserVm)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(WebConstant.AdminRole))
            {
                //order
                return Order(ProdUserVm, claim);
            }
            else
            {
                //inquiry
                return await Inquiry(ProdUserVm, claim);
            }


        }

        private IActionResult Order(ProductUserVM ProdUserVm, Claim? claim)
        {
            var orderTotal = 0.0;
            orderTotal = ProdUserVm.Products.Sum(x => x.QuantityTemp * x.Price);

            var orderHeader = new OrderHeader()
            {
                CreatedByUserId = claim.Value,
                FinalOrderTotal = orderTotal,
                City = ProdUserVm.AppUser.City,
                State = ProdUserVm.AppUser.State,
                Email = ProdUserVm.AppUser.Email,
                PhoneNumber = ProdUserVm.AppUser.PhoneNumber,
                PostalCode = ProdUserVm.AppUser.PostalCode,
                StreetAddress = ProdUserVm.AppUser.StreetAddress,
                FullName = ProdUserVm.AppUser.FullName,
                OrderDate = DateTime.Now,
                OrderStatus = WebConstant.StatusPending
            };

            _ordHdrRep.Add(orderHeader);
            _ordHdrRep.SaveChanges();

            foreach (var prod in ProdUserVm.Products)
            {
                var det = new OrderDetail
                {
                    OrderHeaderId = orderHeader.Id,
                    ProductId = prod.Id,
                    PricePerQuant = prod.Price,
                    Quant = prod.QuantityTemp
                };

                _ordDtlRep.Add(det);
            }
            _ordDtlRep.SaveChanges();

            return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
        }

        private async Task<IActionResult> Inquiry(ProductUserVM ProdUserVm, Claim? claim)
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

            string messageBody = string.Format(HtmlBody,
                                                ProdUserVm.AppUser.FullName,
                                                ProdUserVm.AppUser.Email,
                                                ProdUserVm.AppUser.PhoneNumber,
                                                stringBuilder.ToString());

            await _emailSender.SendEmailAsync(WebConstant.EmailAdmin, Subject, messageBody);

            /*register to db*/
            var hdr = new InquiryHeader
            {
                AppUserId = claim.Value,
                FullName = ProdUserVm.AppUser.FullName,
                Email = ProdUserVm.AppUser.Email,
                Phone = ProdUserVm.AppUser.PhoneNumber,
                InquiryDate = DateTime.Now
            };

            _inqHdrRep.Add(hdr);
            _inqHdrRep.SaveChanges();

            foreach (var prod in ProdUserVm.Products)
            {
                var det = new InquiryDetail
                {
                    InquiryHeaderId = hdr.Id,
                    ProductId = prod.Id
                };

                _inqDtlRep.Add(det);
            }
            _inqDtlRep.SaveChanges();
            TempData[WebConstant.Success] = "Inquiry successfully";


            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader ord = _ordHdrRep.FirstOrDefault(x => x.Id == id);

            HttpContext.Session.Clear();
            return View(ord);
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


        public IActionResult UpdateCart(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            foreach (var product in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = product.Id, QuantityTemp = product.QuantityTemp });
            }
            HttpContext.Session.Set<List<ShoppingCart>>(WebConstant.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }
    }
}
