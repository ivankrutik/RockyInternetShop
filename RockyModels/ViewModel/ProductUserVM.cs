namespace RockyModels.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            Products = new List<Product>();
            AppUser = new AppUser();
        }

        public AppUser AppUser { get; set; }
        public IList<Product> Products { get; set; }
    }
}
