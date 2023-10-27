using System.ComponentModel.DataAnnotations;

namespace SampleInternetShop.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
