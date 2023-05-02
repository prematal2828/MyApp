using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MyApp.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _context;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> products = _context.Product.GetAll(includeProperties: "Category");
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int? ProductId)
        {
            Cart cart = new Cart()
            {
                Product = _context.Product.GetT(x => x.Id == ProductId, includeProperties: "Category"),
                Count = 1,
                ProductId = (int)ProductId,
            };

            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(Cart cart)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claims.Value;

            if (ModelState.IsValid)
            {
                _context.Cart.Add(cart);
                _context.Save();
            }



            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}