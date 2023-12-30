using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.OrderDomain;
using RockyModels.ViewModel;
using RockyUtility;
using RockyUtility.BrainTreeDomain;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _ordHdrRep;
        private readonly IOrderDetailRepository _ordDtlRep;
        private readonly IBrainTreeGate _gate;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IOrderHeaderRepository ordHdrRep,
                                 IOrderDetailRepository ordDtlRep,
                                 IBrainTreeGate gate)
        {
            _ordHdrRep = ordHdrRep;
            _ordDtlRep = ordDtlRep;
            _gate = gate;
        }

        public IActionResult Details(long id)
        {
            OrderVM = new OrderVM()
            {
                Header = _ordHdrRep.Find(id),
                Details = _ordDtlRep.GetAll(x => x.OrderHeaderId == id, includedProperties: "Product")
            };
            return View(OrderVM);
        }

        public IActionResult Index(string? searchName = null, string? searchEmail = null, string? searchPhone = null, string? SelectedStatus = null)
        {
            OrderListVM vm = new OrderListVM()
            {
                Orders = _ordHdrRep.GetAll(x => x.FullName.ToLower().Contains(searchName != null ? searchName.ToLower() : "")
                        && x.Email.ToLower().Contains(searchEmail != null ? searchEmail.ToLower() : "")
                        && x.PhoneNumber.ToLower().Contains(searchPhone != null ? searchPhone.ToLower() : "")
                        && (SelectedStatus == null || x.State == SelectedStatus)),
                Statuses = WebConstant.AllStatuses.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x, Value = x })
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult StartProcessing()
        {
            var header = _ordHdrRep.FirstOrDefault(x => x.Id == OrderVM.Header.Id);
            header.OrderStatus = WebConstant.StatusInProcess;
            _ordHdrRep.SaveChanges();

            TempData[WebConstant.Success] = "Order start success";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ShipOrder()
        {
            var header = _ordHdrRep.FirstOrDefault(x => x.Id == OrderVM.Header.Id);
            header.OrderStatus = WebConstant.StatusShiped;
            header.ShippingDate = DateTime.Now;
            _ordHdrRep.SaveChanges();

            TempData[WebConstant.Success] = "Order shipping success";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CancelOrder()
        {
            var header = _ordHdrRep.FirstOrDefault(x => x.Id == OrderVM.Header.Id);

            var gate = _gate.GetGateWay();
            var transaction = gate.Transaction.Find(header.TransactionId);
            if (transaction.Status == Braintree.TransactionStatus.AUTHORIZED || transaction.Status == Braintree.TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {
                // no refund
                var resultVoid = gate.Transaction.Void(header.TransactionId);
            }
            else
            {
                //refund
                var resultRefund = gate.Transaction.Refund(header.TransactionId);
            }

            header.OrderStatus = WebConstant.StatusRefunded;
            _ordHdrRep.SaveChanges();

            TempData[WebConstant.Success] = "Order canceled success";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateOrderDetails()
        {
            var header = _ordHdrRep.FirstOrDefault(x => x.Id == OrderVM.Header.Id);

            header.FullName = OrderVM.Header.FullName;
            header.PhoneNumber = OrderVM.Header.PhoneNumber;
            header.StreetAddress = OrderVM.Header.StreetAddress;
            header.City = OrderVM.Header.City;
            header.State = OrderVM.Header.State;
            header.PostalCode = OrderVM.Header.PostalCode;
            header.Email = OrderVM.Header.Email;

            _ordHdrRep.SaveChanges();

            TempData[WebConstant.Success] = "Order details updated success";

            return RedirectToAction("Details", "Order", new { id = header.Id });
        }
    }
}
