using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                //check the cookie to confirm that client are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";

                //when sign in this will deal out with cookie
                config.DefaultSignInScheme = "ClientCookie";

                //check if client are allowed to do something
                config.DefaultChallengeScheme = "OurServer";
            })
                    .AddCookie("ClientCookie", x =>
                    {

                    })
                    .AddOAuth("OurServer", x =>
                    {
                        x.ClientId = "client_id";
                        x.ClientSecret = "client_secret";
                        x.CallbackPath = "/oauth/callback/";
                        x.AuthorizationEndpoint = "https://localhost:5001/oauth/authorize";
                        x.TokenEndpoint = "https://localhost:5001/oauth/token";
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
