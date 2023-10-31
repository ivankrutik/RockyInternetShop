using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RockyInternetShop.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greather than 0")]
        public int DisplayOrder { get; set; }
    }
}
