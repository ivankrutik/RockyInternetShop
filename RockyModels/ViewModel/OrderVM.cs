using RockyModels.OrderDomain;

namespace RockyModels.ViewModel
{
    public class OrderVM
    {
        public OrderHeader? Header { get; set; }

        public IEnumerable<OrderDetail>? Details { get; set; }
    }
}
