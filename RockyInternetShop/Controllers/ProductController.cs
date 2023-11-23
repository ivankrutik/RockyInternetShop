﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RockyDataAccess.Data;
using RockyModels;
using RockyModels.ViewModel;
using RockyUtility;

namespace RockyInternetShop.Controllers
{
    [Authorize(Roles = WebConstant.AdminRole)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> Products = _dbContext.Product.Include(x => x.AppType).Include(x => x.Category);
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
                    _dbContext.Product.Add(productVM.Product);
                }
                else
                {
                    var objFromDb = _dbContext.Product.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);

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

                    _dbContext.Product.Update(productVM.Product);
                }

                _dbContext.SaveChanges();
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
            var product = _dbContext.Product.Include(x => x.Category).Include(x => x.AppType).FirstOrDefault(x => x.Id == id);
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
            var product = _dbContext.Product.Find(id);
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

            _dbContext.Product.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        private void FillDefaultValueVM(ProductVM vm)
        {
            vm.CategoryAll = _dbContext.Category.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             });
            vm.AppTypesAll = _dbContext.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }
    }
}
