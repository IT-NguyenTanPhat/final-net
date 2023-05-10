using Microsoft.AspNetCore.Mvc;
using timviec.Models;
using timviec.Utils;

namespace timviec.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db) { _db = db; }
    
        [HttpGet("/admin/categories")]
        public IActionResult Dashboard()
        {
            var email = Request.Cookies["user"];
            if (email != null)
            {
                var user = _db.users.SingleOrDefault(u => u.Email.Equals(email));
                if (user != null && user.Role.Equals("admin"))
                {
                    IEnumerable<Category> categories = _db.categories;
                    return View(categories);
                }
            }
            return View("Error");
        }

        [HttpPost("/admin/categories/create")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Slug.GenerateSlug(category.Name);
                _db.categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Dashboard");
            }
            TempData["error"] = "Thêm danh mục thất bại";
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/admin/categories/{id?}")]
        public IActionResult Update(string? id)
        {
            var email = Request.Cookies["user"];
            if (email != null)
            {
                var user = _db.users.SingleOrDefault(u => u.Email.Equals(email));
                if (user != null && user.Role.Equals("admin"))
                {
                    var category = _db.categories.Find(id);
                    if (category != null)
                    {
                        return View(category);
                    }
                }
            }
            return View("Error");
        }

        [HttpPost("/admin/categories/{id?}")]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Dashboard");
            }
            TempData["error"] = "Cập nhật danh mục thất bại";
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/admin/categories/{id?}/delete")]
        public IActionResult Delete(string? id)
        {
            try
            {
                var obj = _db.categories.Find(id);
                _db.categories.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Xóa danh mục thành công";
                return RedirectToAction("Dashboard");
            }
            catch
            {
                TempData["error"] = "Xóa danh mục thất bại";
                return RedirectToAction("Dashboard");
            }
        }
    }
}
