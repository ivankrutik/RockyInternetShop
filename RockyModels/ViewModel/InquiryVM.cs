using RockyModels.InquiryDomain;

namespace RockyModels.ViewModel
{
    public class InquiryVM
    {
        public InquiryHeader? Header { get; set; }

        public IEnumerable<InquiryDetail>? Details { get; set; }
    }
}
