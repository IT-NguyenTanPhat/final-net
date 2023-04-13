using Microsoft.AspNetCore.Mvc;
using timviec.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace timviec.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db) { _db = db; }

        [HttpGet("/users/{id?}")]
        public IActionResult Profile(string? id)
        {
            var user = _db.users.Find(id);
            if (user != null)
            {
                if (user.Email == Request.Cookies["user"])
                {
                    ViewBag.isOwn = true;
                }
                return View(user);
            }
            return View("Error");
        }

        [HttpPost("/users/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(User user)
        {
            if (ModelState.IsValid)
            {
                _db.users.Update(user);
                _db.SaveChanges();
                TempData["success"] = "Cập nhật hồ sơ thành công";
                return View();
            }
            TempData["error"] = "Cập nhật hồ sơ thất bại";
            return View();
        }
    }
}
