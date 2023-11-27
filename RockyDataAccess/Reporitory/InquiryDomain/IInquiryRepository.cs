using RockyDataAccess.Migrations;
using RockyModels;

namespace RockyDataAccess.Reporitory.Inquiry
{
    public interface IInquiryRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader header);
    }
}
