using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;

namespace MyApp.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _appDbContext;

        public CategoryController(IUnitOfWork appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _appDbContext.Category.GetAll();
            return View(categoryVM);
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
            CategoryVM categoryVM  = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(categoryVM);
            }
            
            else
            {
                categoryVM.Category = _appDbContext.Category.GetT(x => x.Id == id);
                if (categoryVM.Category == null)
                {
                    return NotFound();
                }
                return View(categoryVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM categoryVM)
        {

            if (ModelState.IsValid)
            {
              if (categoryVM.Category.Id == 0)
                {
                    _appDbContext.Category.Add(categoryVM.Category);
                    TempData["success"] = "New Category Created";
                }
              else
                {
                    _appDbContext.Category.Update(categoryVM.Category);
                    TempData["success"] = "Category Updated";
                }

                _appDbContext.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _appDbContext.Category.GetT(x => x.Id == id);
            _appDbContext.Category.Delete(category);
            TempData["error"] = "Deleted";
            _appDbContext.Save();


            return RedirectToAction("Index");
        }
    }
}
