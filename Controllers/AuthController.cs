using Microsoft.AspNetCore.Mvc;
using System.Timers;
using timviec.Models;
using Slug = timviec.Utils.Slug;
using Timer = System.Timers.Timer;

namespace timviec.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        public static dynamic? account;
        private Timer timer;
        private int interval = 60000;

        public AuthController(AppDbContext db) { 
            _db = db;
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += OnTimerElapsed;
        }
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            account = null;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/auth/login")]
        public IActionResult HandleLogin()
        {
            CookieOptions options = new CookieOptions
            {
                HttpOnly= true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Domain = "localhost",
                Path = "/"
            };
            string email = Request.Form["email"].ToString();
            string password = Request.Form["password"].ToString();

            var company = _db.companies.SingleOrDefault(c => c.Email == email && c.Password == password);
            if (company != null)
            {
                Response.Cookies.Append("user", email, options);
                account = company;
                timer.Start();
                return Redirect("/");
            }
            var user = _db.users.SingleOrDefault(c => c.Email == email && c.Password == password);
            if (user != null)
            {
                Response.Cookies.Append("user", email, options);
                account = user;
                timer.Start();
                return Redirect("/");
            }
            
            TempData["error"] = "Email hoặc mật khẩu không hợp lệ";
            return RedirectToAction("Login");
        }

        [HttpGet("/auth/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/auth/register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, Company company)
        {
            if (ModelState.IsValid) {
                var companyEmailChecking = _db.companies.Any(c => c.Email == company.Email);
                var userEmailChecking = _db.users.Any(c => c.Email == user.Email);
                if (companyEmailChecking || userEmailChecking) 
                {
                    TempData["error"] = "Địa chỉ email đã tồn tại";
                    return View();
                }
                
                string role = Request.Form["role"].ToString();
                if (role == "company")
                {
                    company.Id = Slug.GenerateSlug(company.Name);
                    _db.companies.Add(company);
                }
                else
                {
                    user.Id = Slug.GenerateSlug(user.Name);
                    _db.users.Add(user);
                }
                _db.SaveChanges();
                TempData["success"] = "Đăng ký thành công";
                return RedirectToAction("Login");
            }
            TempData["error"] = "Đăng ký thất bại";
            return View();
        }

        [HttpGet("/auth/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Login");
        }
    }
}
