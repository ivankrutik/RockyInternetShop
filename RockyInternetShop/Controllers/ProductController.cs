using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RockyDataAccess.Reporitory.ProductDomain;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;
using System.Security.AccessControl;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _rep;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository rep, IWebHostEnvironment webHostEnvironment)
        {
            _rep = rep;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> Products = _rep.GetAll(includedProperties: "Category,AppType");
            return View(Products);
        }

        public IActionResult Upsert(long? id)
        {
            var ProductVM = new ProductVM()
            {
                Product = new Product(),
            };
            FillDefaultValueVM(ProductVM);

            if (id == null)
            {
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _rep.Find((long)id);
                if (ProductVM.Product != null)
                {
                    return View(ProductVM);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WebConstant.ImgPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var path = Path.Combine(upload, fileName + extension);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = fileName + extension;
                    _rep.Add(productVM.Product);
                }
                else
                {
                    var objFromDb = _rep.FirstOrDefault(filter: x => x.Id == productVM.Product.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstant.ImgPath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.ImageUrl);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        var path = Path.Combine(upload, fileName + extension);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.ImageUrl = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }

                    _rep.Update(productVM.Product);
                }

                _rep.SaveChanges();

                TempData[WebConstant.Success] = "Successfully";

                return RedirectToAction("Index");
            }

            FillDefaultValueVM(productVM);

            return View(productVM);
        }

        public IActionResult Delete(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var product = _rep.FirstOrDefault(filter: x => x.Id == id, includedProperties: "Category,AppType");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(long? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var product = _rep.Find((long)id);
            if (product == null)
            {
                return NotFound();
            }

            if (product.ImageUrl != null)
            {
                string upload = _webHostEnvironment.WebRootPath + WebConstant.ImgPath;
                var file = Path.Combine(upload, product.ImageUrl);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }

            _rep.Remove(product);
            _rep.SaveChanges();

            TempData[WebConstant.Success] = "Deleted successfully";

            return RedirectToAction("Index");
        }


        private void FillDefaultValueVM(ProductVM vm)
        {
            vm.CategoryAll = _rep.GetAllDropDownList(WebConstant.CategoryName);
            vm.AppTypesAll = _rep.GetAllDropDownList(WebConstant.AppTypeName);
        }
    }
}
