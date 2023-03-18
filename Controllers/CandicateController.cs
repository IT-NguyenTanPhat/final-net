using Microsoft.AspNetCore.Mvc;

namespace timviec.Controllers
{
    public class CandicateController : Controller
    {
        [Route("/{id?}")]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
