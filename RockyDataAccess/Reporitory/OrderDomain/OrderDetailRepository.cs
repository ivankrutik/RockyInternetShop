using RockyDataAccess.Data;
using RockyModels.OrderDomain;

namespace RockyDataAccess.Reporitory.OrderDomain
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderDetailRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public void Update(OrderDetail detail)
        {
            _appDbContext.OrderDetail.Update(detail);
        }
    }
}
