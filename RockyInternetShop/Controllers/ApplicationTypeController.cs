using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyInternetShop.Data;
using RockyInternetShop.Models;
using RockyUtility;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ApplicationTypeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> AppType = _dbContext.ApplicationType;
            return View(AppType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ApplicationType.Add(appType);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appType);
        }

        public IActionResult Edit(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var appType = _dbContext.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ApplicationType.Update(appType);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appType);
        }

        public IActionResult Delete(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var appType = _dbContext.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(long? id)
        {
            var appType = _dbContext.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }

            _dbContext.ApplicationType.Remove(appType);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
