using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class OauthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorize(string username)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Token()
        {
            return View();
        }
    }
}
