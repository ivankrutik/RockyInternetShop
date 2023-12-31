﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockyModels
{
    public class Product
    {
        public Product()
        {
            QuantityTemp = 1;
        }

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
        public virtual Category? Category { get; set; }

        [Display(Name = "Application Type")]
        public long AppTypeId { get; set; }
        [ForeignKey(nameof(AppTypeId))]
        public virtual ApplicationType? AppType { get; set; }

        [Range(1, 10000, ErrorMessage = "Quant must be more than 0")]
        [NotMapped]
        public int QuantityTemp { get; set; }
    }
}
