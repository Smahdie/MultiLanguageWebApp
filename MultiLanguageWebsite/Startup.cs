using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace MultiLanguageWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] {
                    new CultureInfo("fa"),
                    new CultureInfo("en")
                };
                options.DefaultRequestCulture = new RequestCulture("fa");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                var provider = new RouteValueRequestCultureProvider { Options = options };

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    provider
                };
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            //other configurations here
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                    pattern: "{culture}/{controller=Home}/{action=Index}/{id?}",
                    constraints: new { cultureConstraint = new CultureRouteConstraint() }
                );

                endpoints.MapControllerRoute(
                    name: "defaultNoCulture",
                   pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
