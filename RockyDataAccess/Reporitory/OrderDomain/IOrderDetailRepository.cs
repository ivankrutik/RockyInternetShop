using RockyModels.OrderDomain;

namespace RockyDataAccess.Reporitory.OrderDomain
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail detail);
    }
}
