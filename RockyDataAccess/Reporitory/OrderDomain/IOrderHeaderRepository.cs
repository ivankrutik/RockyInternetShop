using RockyModels.OrderDomain;

namespace RockyDataAccess.Reporitory.OrderDomain
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader header);
    }
}
