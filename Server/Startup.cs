using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {

                    //pass token through query
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            string accessToken = "access_token";
                            if (context.Request.Query.ContainsKey(accessToken))
                                context.Token = context.Request.Query[accessToken];

                            return Task.CompletedTask;
                        }
                    };


                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Constants.Issuer,
                        ValidAudience = Constants.Audiance,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret))
                    };
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
