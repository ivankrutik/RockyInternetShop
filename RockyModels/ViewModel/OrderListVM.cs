using Microsoft.AspNetCore.Mvc.Rendering;
using RockyModels.OrderDomain;

namespace RockyModels.ViewModel
{
    public class OrderListVM
    {
        public IEnumerable<OrderHeader> Orders { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        public string SelectedStatus { get; set; }
    }
}
