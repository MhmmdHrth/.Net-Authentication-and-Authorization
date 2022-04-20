using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public async Task<IActionResult> Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie")
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)),
                                                            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Constants.Issuer,
                                             Constants.Audiance,
                                             claims,
                                             DateTime.Now,
                                             DateTime.Now.AddHours(1),
                                             signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {accessToken = tokenJson});
        }

        public IActionResult Decode(string part)
        {
            var bytes = Convert.FromBase64String(part);
            return Ok(Encoding.UTF8.GetString(bytes));
        }
    }
}
