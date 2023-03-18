using Microsoft.AspNetCore.Mvc;

namespace timviec.Controllers
{
    public class CompanyController : Controller
    {
        [Route("/companies/{id?}")]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
