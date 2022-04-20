using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public OperationsController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public async Task<IActionResult> Open()
        {
            var resource = new CookieJar(); //get from Db

            await authorizationService.AuthorizeAsync(User, resource, CookieJarAuthOperations.Open);
            return View();
        }
    }

    public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, CookieJar>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       CookieJar resource)
        {
            switch (requirement.Name)
            {
                case CookieJarOperations.Look:
                    if(context.User.Identity.IsAuthenticated)
                        context.Succeed(requirement);
                    break;

                case CookieJarOperations.ComeNear:
                    if (context.User.HasClaim("Friend", "Good"))
                        context.Succeed(requirement);
                    break;
            }
            return Task.CompletedTask;
        }
    }

    public static class CookieJarAuthOperations
    {
        public static OperationAuthorizationRequirement Open = new OperationAuthorizationRequirement
        {
            Name = CookieJarOperations.Open
        };
    }

    public static class CookieJarOperations
    {
        public const string Open = "Open";
        public const string TakeCookie = "TakeCookie";
        public const string ComeNear = "ComeNear";
        public const string Look = "Look";
    }

    public class CookieJar { public string Name { get; set; }}
}
