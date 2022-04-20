using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("Memory"));
            services.AddIdentity<IdentityUser, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 4;
                x.Password.RequireDigit = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(x =>
            {
                x.Cookie.Name = "Identity_Cookies";
                x.LoginPath = "/Home/Login";
            });

            services.AddControllersWithViews();
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
