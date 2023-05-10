using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timviec.Models;
using timviec.Utils;
using static timviec.Controllers.HomeController;

namespace timviec.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public UserController(AppDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

        [HttpGet("/users/{id?}")]
        public IActionResult Profile(string? id, int? p)
        {
            var user = _db.users.Include(u => u.Applies).SingleOrDefault(u => u.Id.Equals(id));
            if (user != null)
            {
                int count = user.Applies.Count;

                int pageSize = 6; // number of items per page
                int pageNumber = (p ?? 1);

                user = _db.users
                    .Include(u => u.Applies.Skip((pageNumber - 1) * pageSize).Take(pageSize))
                    .ThenInclude(a => a.Job)
                    .ThenInclude(j => j.Company)
                    .SingleOrDefault(u => u.Id.Equals(id));

                Database model = new Database()
                {
                    user = user,
                    page = new Page(pageNumber, (int)Math.Ceiling((float)count / pageSize))
                };
                return View(model);
            }
            return View("Error");
        }

        [HttpPost("/users/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(User newUser, IFormFile avatar, IFormFile cv)
        {
            try
            {
                var oldUser = _db.users.Find(newUser.Id);

                if (oldUser == null)
                {
                    return View("Error");
                }

                var properties = newUser.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var newValue = property.GetValue(newUser);
                    if (newValue != null)
                    {
                        property.SetValue(oldUser, newValue);
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
                    oldUser.Avatar = "/uploads/" + uniqueFileName;

                    // Combine the path where you want to store the file with the unique filename
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", uniqueFileName);

                    // Save the file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }
                }

                // Check if a file was actually uploaded
                if (cv != null)
                {
                    // Get the filename and extension
                    var fileName = Path.GetFileName(cv.FileName);
                    var fileExtension = Path.GetExtension(fileName);

                    // Generate a unique filename to prevent overwriting existing files
                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    oldUser.CV = "/uploads/" + uniqueFileName;

                    // Combine the path where you want to store the file with the unique filename
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", uniqueFileName);

                    // Save the file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        cv.CopyTo(stream);
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
    }
}
