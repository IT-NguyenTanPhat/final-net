using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;
using timviec.Utils;

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
            public IEnumerable<Apply> applies { get; set; }
            public User user { get; set; }
            public Page page { get; set; }
        }

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Route("")]
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.categories.Include(c => c.Jobs.Where(j => j.Status.Equals("censored"))).Take(4).ToList();
            IEnumerable<Job> jobs = _db.jobs.Include(c => c.Company).Where(j => j.Status.Equals("censored")).Take(6).ToList();
            IEnumerable<Company> companies = _db.companies.Take(8).ToList();

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
            var email = Request.Cookies["user"];
            if (email != null)
            {
                var user = _db.users.SingleOrDefault(u => u.Email.Equals(email));
                if (user != null && user.Role.Equals("admin"))
                {
                    return RedirectToAction("Dashboard", "Category");
                }
            }
            return View("Error");
        }

        [Route("{*url}")]
        public IActionResult Error()
        {
            ViewData["message"] = "404 Not Found";
            return View();
        }
    }
}