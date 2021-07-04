using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace MultiLanguageWebApp
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
                //Part 1: support English and Persian languages
                var supportedCultures = new[] {
                    new CultureInfo("fa"),
                    new CultureInfo("en")
                };
                options.DefaultRequestCulture = new RequestCulture("fa");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Part 2: introduce a way to detect current culture
                var provider = new RouteValueRequestCultureProvider { Options = options };
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    provider
                };
            });

            //Part 3: Tell the app to look for resource files in the **Resources** folder
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //Part 4: Add a constraint to routeOptions, to check the culture with **CultureRouteConstraint** class
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
            });

            services.AddMvc()
                //Part 5: Support localization in View files
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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //Part 6
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
                //Part 7: introduce a routing that supports culture
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
