using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;
using static timviec.Controllers.HomeController;

namespace timviec.Controllers
{
    public class JobController : Controller
    {
        private readonly AppDbContext _db;
        public JobController(AppDbContext db)
        {
            _db = db;
        }

        [Route("/jobs")]
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.categories;
            IEnumerable<Job> jobs = _db.jobs.Include(c => c.Company).Include(c => c.Category);

            Database model = new Database()
            {
                categories = categories,
                jobs = jobs,
            };
            return View(model);
        }

        [HttpGet("/jobs/{id?}")]
        public IActionResult Detail(string? id)
        {
            var job = _db.jobs.Include(j => j.Company).Include(j => j.Category).SingleOrDefault(j => j.Id == id);
            if (job != null)
            {
                return View(job);
            }
            return View("Error");
        }

        [HttpPost("/jobs/{id?}")]
        public IActionResult Apply()
        {
            return View();
        }

        [HttpGet("/admin/jobs")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
