using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.OrderDomain;
using RockyModels.ViewModel;
using RockyUtility;
using RockyUtility.BrainTreeDomain;

namespace RockyInternetShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _ordHdrRep;
        private readonly IOrderDetailRepository _ordDtlRep;
        private readonly IBrainTreeGate _gate;

        public OrderController(IOrderHeaderRepository ordHdrRep,
                                 IOrderDetailRepository ordDtlRep,
                                 IBrainTreeGate gate)
        {
            _ordHdrRep = ordHdrRep;
            _ordDtlRep = ordDtlRep;
            _gate = gate;
        }

        public IActionResult Index()
        {
            OrderListVM vm = new OrderListVM()
            {
                Orders = _ordHdrRep.GetAll(),
                Statuses = WebConstant.AllStatuses.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x, Value = x }),
                SelectedStatus = WebConstant.StatusApproved
            };

            return View(vm);
        }
    }
}
