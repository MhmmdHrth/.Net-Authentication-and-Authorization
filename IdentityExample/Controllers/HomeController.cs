using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> usermanager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager)
        {
            this.usermanager = usermanager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await usermanager.FindByNameAsync(username);
            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, false, false);
                if(result.Succeeded)
                    return RedirectToAction(nameof(Secret));
            }
             
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser 
            { 
                UserName = username,
                Email = "",
                PasswordHash = "custom hash"
            };

            var result = await usermanager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var signin = await signInManager.PasswordSignInAsync(user, password, false, false);
                if(signin.Succeeded)
                    return RedirectToAction(nameof(Secret));
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
