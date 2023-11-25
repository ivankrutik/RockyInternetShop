using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyDataAccess.Reporitory.AppTypeDomain;
using RockyModels;
using RockyUtility;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IAppTypeRepository _rep;

        public ApplicationTypeController(IAppTypeRepository rep)
        {
            _rep = rep;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> AppType = _rep.GetAll();
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
                _rep.Add(appType);
                _rep.SaveChanges();
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
            var appType = _rep.Find((long)id);
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
                _rep.Update(appType);
                _rep.SaveChanges();
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
            var appType = _rep.Find((long)id);
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
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var appType = _rep.Find((long)id);
            if (appType == null)
            {
                return NotFound();
            }

            _rep.Remove(appType);
            _rep.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
