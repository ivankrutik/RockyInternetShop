using RockyDataAccess.Data;
using RockyModels.InquiryDomain;

namespace RockyDataAccess.Reporitory.InquiryDomain
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly AppDbContext _appDbContext;

        public InquiryDetailRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(InquiryDetail detail)
        {
            _appDbContext.InquiryDetail.Update(detail);
        }
    }
}
