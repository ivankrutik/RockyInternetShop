using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RockyInternetShop.Data;
using RockyInternetShop.Models;
using RockyInternetShop.Models.ViewModel;

namespace RockyInternetShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> Products = _dbContext.Product;

            foreach (Product product in Products)
            {
                product.Category = _dbContext.Category.FirstOrDefault(x => x.Id == product.CategoryId);
            }

            return View(Products);
        }

        public IActionResult Upsert(long? id)
        {
            var ProductVM = new ProductVM()
            {
                Product = new Product(),
                CategoryAll = _dbContext.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _dbContext.Product.Find(id);
                if (ProductVM.Product != null)
                {
                    return View(ProductVM);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Category.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _dbContext.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(long? id)
        {
            var category = _dbContext.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Category.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
