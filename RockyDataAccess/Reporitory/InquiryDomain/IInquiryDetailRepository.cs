using RockyModels.InquiryDomain;

namespace RockyDataAccess.Reporitory.InquiryDomain
{
    public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail detail);
    }
}
