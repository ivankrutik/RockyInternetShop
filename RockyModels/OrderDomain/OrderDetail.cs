using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockyModels.OrderDomain
{
    public class OrderDetail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }


        [Required]
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quant { get; set; }
        public double PricePerQuant { get; set; }
    }
}