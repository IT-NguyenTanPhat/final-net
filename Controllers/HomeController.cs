using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using timviec.Models;

namespace timviec.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Route("")]
        public IActionResult Index()
        {
            IEnumerable<Category> obj = _db.categories;
            return View(obj);
        }

        [Route("/jobs")]
        public IActionResult Jobs()
        {
            IEnumerable<Category> obj = _db.categories;
            return View(obj);
        }

        [Route("/jobs/{id?}")]
        public IActionResult JobDetail()
        {
            return View();
        }

        [Route("/jobs/{id?}/apply")]
        public IActionResult ApplyForm()
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