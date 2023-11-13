namespace RockyInternetShop.Models.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            Products = new List<Product>();
            AppUser = new AppUser();
        }

        public AppUser AppUser { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
