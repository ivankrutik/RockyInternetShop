using RockyModels.InquiryDomain;

namespace RockyDataAccess.Reporitory.InquiryDomain
{
    public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader header);
        void RemoveWithDeatails(InquiryHeader header, IInquiryDetailRepository detailRepository);
    }
}
