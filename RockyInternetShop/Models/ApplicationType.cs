using System.ComponentModel.DataAnnotations;

namespace RockyInternetShop.Models
{
    public class ApplicationType
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
