using SmartReservationCinema.FilmContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartReservationCinema.Services;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Http;

namespace SmartReservationCinema
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddControllersWithViews();
            services.AddSession();
            services.AddDbContext<FilmDbContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,config =>
                    {
                        config.LoginPath = "/Account/Login";
                        config.AccessDeniedPath = "/Account/Denied";
                    });
            services.AddAuthentication();
            services.AddSingleton<MailSender>();
            //            services.AddAuthentication(o =>
            //            {
            //                // This forces challenge results to be handled by Google OpenID Handler, so there's no
            //                // need to add an AccountController that emits challenges for Login.
            //                //o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //                // This forces forbid results to be handled by Google OpenID Handler, which checks if
            //                // extra scopes are required and does automatic incremental auth.
            //                //o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //                // Default scheme that will handle everything else.
            //                // Once a user is authenticated, the OAuth2 token info is stored in cookies.
            //                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //            })
            ////            .AddCookie()

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            })
        //.AddCookie()
        .AddGoogleOpenIdConnect(options =>
            {
                options.ClientId = "182804071684-hj1e18ooihigcis8177n41klt0jdnf55.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Film}/{action=Index}/{id?}");
            });
        }
    }
}
