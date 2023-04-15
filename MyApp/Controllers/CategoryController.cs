using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _appDbContext;

        public CategoryController(IUnitOfWork appDbContext)
        {
            _appDbContext=appDbContext;
        }
        public IActionResult Index()
        {
            var categories = _appDbContext.Category.GetAll();
            return View(categories);
        }
        [HttpGet]
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
                _appDbContext.Category.Add(category);
                _appDbContext.Save();
                TempData["success"] = "New Category Created";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id==null || id == 0)
            {
                return NotFound();
            }
            var category = _appDbContext.Category.GetT(x => x.Id==id);
            if (category==null)
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
                _appDbContext.Category.Update(category);
                _appDbContext.Save();
                TempData["success"] = "Category Updated";
                return RedirectToAction("Index");
            }
            return View("Index");

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {

            if (id==null || id == 0)
            {
                return NotFound();
            }
            var category = _appDbContext.Category.GetT(x => x.Id==id);
            _appDbContext.Category.Delete(category);
            TempData["error"] = "Deleted";
            _appDbContext.Save();


            return RedirectToAction("Index");
        }
    }
}
