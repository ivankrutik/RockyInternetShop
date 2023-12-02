using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.InquiryDomain;

namespace RockyInternetShop.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _headerRepository;
        private readonly IInquiryDetailRepository _detailRepository;

        public InquiryController(IInquiryHeaderRepository headerRepository, IInquiryDetailRepository detailRepository)
        {
            _detailRepository = detailRepository;
            _headerRepository = headerRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _headerRepository.GetAll() });
        }
    }
}
