using Basics.Controllers;
using Basics.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Basics
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                    .AddCookie("CookieAuth", x =>
                    {
                        x.Cookie.Name = "Harith_Cookies";
                        x.LoginPath = "/Home/Authenticate";
                    });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Basic.Policy", policy =>
                {
                    policy.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddControllersWithViews(x =>
            {
                //globalAuthFilter
                var defaultPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                
                x.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
