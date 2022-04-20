using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Basic.Policy")]
        public IActionResult SecretPolicy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Authenticate()
        {
            var harithClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Harith"),
                new Claim(ClaimTypes.Email, "harith@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.DateOfBirth, "02/10/1999")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Muhammad Harith Jamdil Ashari"),
                new Claim("DrivingLicense", "B2")
            };

            var harithIdentity = new ClaimsIdentity(harithClaims, "Harith Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] {harithIdentity, licenseIdentity });


            await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction(nameof(Secret));
        }

        public async Task<IActionResult> doStuff([FromServices] IAuthorizationService authorizationService)
        {
            var builder = new AuthorizationPolicyBuilder("Scheme");
            var customPolicy = builder.RequireClaim("Hello").Build();

            await authorizationService.AuthorizeAsync(User, customPolicy);
            return View(nameof(Index));
        }
    }
}
