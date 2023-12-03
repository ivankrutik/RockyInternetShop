using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.InquiryDomain;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;

namespace RockyInternetShop.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _headerRepository;
        private readonly IInquiryDetailRepository _detailRepository;
        [BindProperty]
        public InquiryVM InqVM { get; set; }

        public InquiryController(IInquiryHeaderRepository headerRepository, IInquiryDetailRepository detailRepository)
        {
            _detailRepository = detailRepository;
            _headerRepository = headerRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            var carts = new List<ShoppingCart>();

            InqVM.Details = _detailRepository.GetAll(x => x.InquiryHeaderId == InqVM.Header.Id);
            foreach (var detail in InqVM.Details)
            {
                var cart = new ShoppingCart() { ProductId = detail.ProductId };
                carts.Add(cart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WebConstant.SessionCart, carts);
            HttpContext.Session.Set(WebConstant.SessionInquiryId, InqVM.Header.Id);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Details(long id)
        {
            InqVM = new InquiryVM
            {
                Header = _headerRepository.FirstOrDefault(x => x.Id == id),
                Details = _detailRepository.GetAll(x => x.InquiryHeaderId == id, includedProperties: "Product")
            };

            return View(InqVM);
        }

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _headerRepository.GetAll() });
        }
    }
}
