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

        [HttpGet("/{id?}")]
        public IActionResult Profile(string? id)
        {
            if (_db.users.Find(id) != null)
            {
                return RedirectToAction("Profile", "User", new { id });

            }
            if (_db.companies.Find(id) != null)
            {
                return RedirectToAction("Profile", "Company", new { id });

            }
            return View("Error");
        }

        [HttpGet("/find-account")]
        public IActionResult FindAccount()
        {
            if (Request.Query.TryGetValue("email", out var value))
            {
                var email = value.ToString();
                var user = _db.users
                    .Where(u => u.Email == email)
                    .Select(u => new { u.Id, u.Name, u.Avatar, u.Email })
                    .SingleOrDefault();
                if (user != null)
                {
                    return Json(user);
                }

                var company = _db.companies
                    .Where(u => u.Email == email)
                    .Select(u => new { u.Id, u.Name, u.Avatar, u.Email })
                    .SingleOrDefault();
                if (company != null)
                {
                    return Json(company);
                }
            }
            return BadRequest();
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