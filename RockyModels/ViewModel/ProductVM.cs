﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace RockyModels.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem>? CategoryAll { get; set; }
        public IEnumerable<SelectListItem>? AppTypesAll { get; set; }
    }
}
