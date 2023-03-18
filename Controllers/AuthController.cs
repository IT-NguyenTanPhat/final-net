using Microsoft.AspNetCore.Mvc;

namespace timviec.Controllers
{
    public class AuthController : Controller
    {
        [Route("/auth/login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/auth/register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
