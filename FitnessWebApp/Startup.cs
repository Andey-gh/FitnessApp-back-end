using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FitnessWebApp.Domain;
using Microsoft.OpenApi.Models;
using FitnessWebApp.Models;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FitnessWebApp.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FitnessWebApp
{
    public class Startup
    {
        public Startup(IConfiguration ñonfiguration)
        {
            Configuration = ñonfiguration;
        }

        public IConfiguration Configuration { get;}



            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services){
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddDbContext<AppDbContext>(options => options.UseMySql(Configuration.GetConnectionString("Release")));
            
            //íàñòðàèâàåì identity ñèñòåìó
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.SignIn.RequireConfirmedEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
                opts.Tokens.PasswordResetTokenProvider = ResetPasswordTokenProvider.ProviderKey;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddTokenProvider<ResetPasswordTokenProvider>(ResetPasswordTokenProvider.ProviderKey);
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "fitnessWebApp";
                options.Cookie.HttpOnly = false;
                options.LoginPath = "/api/login";
                options.AccessDeniedPath = "/";
                options.LogoutPath = "/api/Logout";
                options.SlidingExpiration = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.ExpireTimeSpan= TimeSpan.FromHours(24);

            });
           
            services.AddControllersWithViews();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
           
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute(name: "login",pattern: "login/",defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "pre-session", pattern: "pre-session/", defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "progress", pattern: "progress/", defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "my-training-plan", pattern: "my-training-plan/", defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "training-history", pattern: "training-history/", defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "settings", pattern: "settings/", defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
