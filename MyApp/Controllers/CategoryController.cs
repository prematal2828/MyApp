using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }
        public IActionResult Index()
        {
            var categories = _appDbContext.Categories.ToList();
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
            if(ModelState.IsValid)
            {
                _appDbContext.Categories.Add(category);
                _appDbContext.SaveChanges();
                TempData["success"] = "New Category Created";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if(id==null || id == 0)
            {
                return NotFound();
            }
            var category = _appDbContext.Categories.Find(id);
            if(category==null)
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
                _appDbContext.Categories.Update(category);
                _appDbContext.SaveChanges();
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
            var category = _appDbContext.Categories.Find(id);
            _appDbContext.Categories.Remove(category);
            TempData["error"] = "Deleted";
            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
