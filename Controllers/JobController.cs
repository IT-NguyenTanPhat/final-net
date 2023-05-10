using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;
using timviec.Utils;
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
        public IActionResult Index(string? s, string? c, int? p)
        {
            int pageSize = 6; // number of items per page
            int pageNumber = (p ?? 1);
            IEnumerable<Category> categories = _db.categories;
            IEnumerable<Job> jobs;
            if (s != null || c != null) {
                jobs = _db.jobs
                .Where(j => (
                    j.Name.Contains(s) || j.Company.Name.Contains(s) || j.CategoryId.Equals(c)) 
                    && j.Status.Equals("censored"))
                .Include(c => c.Company)
                .Include(c => c.Category);

                ViewBag.search = s;
                ViewBag.category = c;
            } 
            else
            {
                jobs = _db.jobs
                .Include(c => c.Company)
                .Include(c => c.Category)
                .Where(j => j.Status.Equals("censored"));
            }
            int count = jobs.Count();

            jobs = jobs.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            Database model = new Database()
            {
                categories = categories,
                jobs = jobs,
                page = new Page(pageNumber, (int)Math.Ceiling((float)count / pageSize))
            }; 

            return View(model);
        }

        [HttpGet("/jobs/{id?}")]
        [HttpGet("/admin/jobs/{id?}")]
        public IActionResult Detail(string? id)
        {
            IEnumerable<Job> jobs = new List<Job>();
            var job = _db.jobs.Include(j => j.Company).Include(j => j.Category).SingleOrDefault(j => j.Id == id);
            if (job == null)
            {
                return View("Error");
            }

            IEnumerable<Apply> applies = _db.applies
                .Where(a => a.JobId.Equals(id) && a.Status.Equals("pending"))
                .Include(j => j.Job)
                .Include(j => j.User);
            Database database = new Database()
            {
                jobs = jobs.Append(job),
                categories = _db.categories,
                applies = applies
            };
            return View(database);
        }

        [HttpPost("/jobs/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(string? id, Job newJob)
        {
            try
            {
                var oldJob = _db.jobs.Find(newJob.Id);
                if (oldJob == null)
                {
                    return View("Error");
                }

                var properties = newJob.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var newValue = property.GetValue(newJob);
                    if (newValue != null)
                    {
                        property.SetValue(oldJob, newValue);
                    }
                }

                _db.SaveChanges();
                TempData["success"] = "Cập nhật thành công";
                return Redirect($"/jobs/{id}");
            }
            catch
            {
                TempData["error"] = "Vui lòng thử lại";
                return Redirect($"/jobs/{id}");
            }
        }

        [HttpPost("/jobs/{id?}/apply")]
        public IActionResult Apply(string? id)
        {
            var email = Request.Cookies["user"];
            if (email == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = _db.users.SingleOrDefault(u => u.Email.Equals(email));
            if (user != null)
            {
                Apply apply = new Apply()
                {
                    JobId = id,
                    UserId = user.Id,
                    CoverLetter = Request.Form["cover-letter"]
                };
                _db.applies.Add(apply);
                _db.SaveChanges();
                TempData["success"] = "Ứng tuyển thành công";
                return Redirect($"/jobs/{id}");
            }
            ViewData["message"] = "Tính năng này chỉ dành cho ứng viên.";
            return View("Error");
        }

        [HttpGet("/admin/jobs")]
        public IActionResult Censor()
        {
            var email = Request.Cookies["user"];
            if (email != null)
            {
                var user = _db.users.SingleOrDefault(u => u.Email.Equals(email));
                if (user != null && user.Role.Equals("admin"))
                {
                    var jobs = _db.jobs.Where(j => j.Status == "pending").Include(j => j.Company);
                    return View(jobs);
                }
            }
            return View("Error");
        }

        [HttpPost("/admin/jobs/{id?}")]
        public IActionResult Censor(string? id)
        {
            var job = _db.jobs.Find(id);
            if (job == null)
            {
                return View("Error");
            }
            var option = Request.Form["option"];
            job.Status = option;
            _db.SaveChanges();
            return Redirect("/admin/jobs");
        }
    }
}
