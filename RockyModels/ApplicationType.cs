using System.ComponentModel.DataAnnotations;

namespace RockyModels
{
    public class ApplicationType
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
