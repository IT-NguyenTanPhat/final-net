using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;

namespace timviec.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public class Database {
            public IEnumerable<Category> categories { get; set; }
            public IEnumerable<Job> jobs { get; set; }
            public IEnumerable<Company> companies { get; set; }
        }

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Route("")]
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.categories.Include(c => c.Jobs);
            IEnumerable<Job> jobs = _db.jobs.Include(c => c.Company);
            IEnumerable<Company> companies = _db.companies;

            Database model = new Database()
            {
                categories = categories,
                jobs = jobs,
                companies = companies,
            };
            return View(model);
        }

        [HttpGet("/get-locations")]
        public IActionResult GetLocations()
        {
            var locations = _db.companies
                .Select(u => new { u.Locaiton });
            if (locations != null)
            {
                return Json(locations);
            }
            return BadRequest();
        }

        [HttpGet("/admin")]
        public IActionResult Admin()
        {
            return RedirectToAction("Dashboard", "Category");
        }

        [Route("{*url}")]
        public IActionResult Error()
        {
            ViewData["message"] = "404 Not Found";
            return View();
        }
    }
}