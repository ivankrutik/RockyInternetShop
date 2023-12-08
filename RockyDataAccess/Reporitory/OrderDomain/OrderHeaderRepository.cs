using RockyDataAccess.Data;
using RockyModels.OrderDomain;

namespace RockyDataAccess.Reporitory.OrderDomain
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderHeaderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public void Update(OrderHeader header)
        {
            _appDbContext.OrderHeader.Update(header);
        }
    }
}
