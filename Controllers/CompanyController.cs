using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;
using timviec.Utils;

namespace timviec.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CompanyController(AppDbContext db, IWebHostEnvironment env) {
            _db = db;
            _env = env;
        }

        [HttpGet("/companies/{id?}")]
        public IActionResult Profile(string? id)
        {
            var company = _db.companies
                     .Include(c => c.Jobs)
                     .ThenInclude(j => j.Category)
                     .FirstOrDefault(c => c.Id == id);

            if (company != null)
            {
                if (Request.Cookies["user"] == company.Email)
                {
                    ViewBag.isOwn = true;
                }
                return View(company);
            }
            ViewData["message"] = "Xin lỗi, không tìm thấy công ty này.";
            return View("Error");
        }

        [HttpPost("/companies/{Id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(Company updatedCompany, IFormFile avatar)
        {
            try
            {
                var originalCompany =  _db.companies.Find(updatedCompany.Id);

                if (originalCompany == null)
                {
                    return View("Error");
                }

                var properties = updatedCompany.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var newValue = property.GetValue(updatedCompany);
                    if (newValue != null)
                    {
                        property.SetValue(originalCompany, newValue);
                    }
                }

                // Check if a file was actually uploaded
                if (avatar != null)
                {
                    // Get the filename and extension
                    var fileName = Path.GetFileName(avatar.FileName);
                    var fileExtension = Path.GetExtension(fileName);

                    // Generate a unique filename to prevent overwriting existing files
                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    originalCompany.Avatar = "/uploads/" + uniqueFileName;

                    // Combine the path where you want to store the file with the unique filename
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", uniqueFileName);

                    // Save the file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }
                }   
                _db.SaveChanges();
                TempData["success"] = "Cập nhật hồ sơ thành công";
                return RedirectToAction("Profile");
            }
             catch
            {
                TempData["error"] = "Cập nhật hồ sơ thất bại";
                return RedirectToAction("Profile");
            }
        }

        [HttpGet("/companies/{id?}/post")]
        public IActionResult PostJob(string? id)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var company = _db.companies.Find(id);
            if (company != null)
            {
                ViewData["CompanyName"] = company.Name;
                ViewData["CompanyId"] = company.Id;
                IEnumerable<Category> categories = _db.categories;
                return View(categories);
            }
            ViewData["message"] = "Tính năng này chỉ dành cho doanh nghiệp.";
            return View("Error");
        }

        [HttpPost("/companies/{id?}/post")]
        [ValidateAntiForgeryToken]
        public IActionResult PostJob(string? id, Job job)
        {
            if (ModelState.IsValid)
            {
                job.Id = Slug.GenerateSlug(job.Name);
                job.CompanyId = id;
                job.CategoryId = Request.Form["category"].ToString();
                _db.jobs.Add(job);
                _db.SaveChanges();
                TempData["success"] = "Đăng tin thành công";
                return Redirect($"/companies/{id}");
            }
            TempData["error"] = "Vui lòng thử lại";
            return RedirectToAction("PostJob");
        }
    }
}
