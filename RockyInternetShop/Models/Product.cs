using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockyInternetShop.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Category Type")]
        public long CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
