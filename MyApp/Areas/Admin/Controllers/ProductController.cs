using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.DataAccessLayer.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;


namespace MyApp.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _appDbContext;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment=webHostEnvironment;
        }

        #region APICALL
        public IActionResult AllProducts()
        {
            var products = _appDbContext.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {
            ProductVM productVM = new ProductVM();
            productVM.Products = _appDbContext.Product.GetAll();
            return View(productVM);
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _appDbContext.Category.Add(category);
        //        _appDbContext.Save();
        //        TempData["success"] = "New Category Created";
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new(),
                Categories = _appDbContext.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
            };

            if (id == null || id == 0)
            {

                return View(productVM);
            }

            else
            {
                productVM.Product = _appDbContext.Product.GetT(x => x.Id == id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if (file!=null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    fileName = Guid.NewGuid().ToString()+"-"+file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Image\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _appDbContext.Product.Add(productVM.Product);
                    TempData["success"] = "New Product Created";
                }
                else
                {
                    _appDbContext.Product.Update(productVM.Product);
                    TempData["success"] = "Product Updated";
                }

                _appDbContext.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        #region DeleteAPICALL
        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _appDbContext.Product.GetT(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error in fetching data" });
            }
            else
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _appDbContext.Product.Delete(product);
                TempData["error"] = "Deleted";
                _appDbContext.Save();
                return Json(new { success = true, message = "Product Deleted" });
            }
        }
        #endregion
    }
}
