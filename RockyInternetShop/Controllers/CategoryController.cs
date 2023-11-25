using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.CategoryDomain;
using RockyModels;
using RockyUtility;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _rep;

        public CategoryController(ICategoryRepository rep)
        {
            _rep = rep;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _rep.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _rep.Add(category);
                _rep.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _rep.Find((long)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _rep.Update(category);
                _rep.SaveChanges();
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
            var category = _rep.Find((long)id);
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
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _rep.Find((long)id);
            if (category == null)
            {
                return NotFound();
            }

            _rep.Remove(category);
            _rep.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
