using Microsoft.AspNetCore.Mvc.Rendering;

namespace RockyInternetShop.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoryAll { get; set; }
    }
}
