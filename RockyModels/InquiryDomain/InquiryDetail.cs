using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockyModels.InquiryDomain
{
    public class InquiryDetail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long InquiryHeaderId { get; set; }
        [ForeignKey(nameof(InquiryHeaderId))]
        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        public long ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
