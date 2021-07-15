![License](https://img.shields.io/github/license/Smahdie/MultiLanguageWebApp?style=flat-square)


This is a sample project that shows how to support multiple languages in an ASP.NET web application. The project is created using .NET 5 and MVC framework.

# 1. Startup

First, we need to configure our startup file to handle request localization.
## 1.1. ConfigureServices method 

First we introduce the cultures we want to support, and the default culture. Then we introduce *RouteValueRequestCultureProvider* as a means for detecting request culture, which is a class inside the project.

```       
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
```
Next, we tell the app to look for resource files in the *Resources* folder.
```
services.AddLocalization(options => options.ResourcesPath = "Resources");
```
Then we add a constraint to routeOptions, to check the culture with *CultureRouteConstraint* class, which is a class inside the project. It checks the request culture and rejects requests with unsupported cultures.
```
services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
});
```
Finally, we add this configuration lines to support localization in View files:
```
services.AddMvc()
    //Part 5: Support localization in View files
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
```

## 1.2. Configure method
Again, we add these lines to support request localization:
```
var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
```

And define a routing that supports culture
```
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
```
# 2. Resource Files

## 2.1. Shared Resource

 We can have a shared resource which is available in all of views and controllers. To support shared resource, we need to introduce a dummy class. In this project I named it *SharedResource*. Then we can create resource files which have the same name as our dummy class.
 
 ![image](https://user-images.githubusercontent.com/5062683/124382468-d5a1d200-dcdc-11eb-80b6-3c39c7c20f85.png)
 
**Use Shared Resource inside a View**
 ```
@using Microsoft.AspNetCore.Mvc.Localization
@inject IHtmlLocalizer<SharedResource> SharedLocalizer

<h1 class="display-4">@SharedLocalizer["Welcome"]</h1>
```

**Use Shared Resource inside a Controller**
 ```
private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

public IActionResult Index()
{
    var welcomeMsg = _sharedLocalizer["Welcome"];
    return View(welcomeMsg);
}
 ```

## 2.2. Specific Resource files

We can have specific resource file for each view, model and controller. Each resource file should have the same name of the original file and be inside the same folder.

![image](https://user-images.githubusercontent.com/5062683/124383312-11d73180-dce1-11eb-82c4-d16c7b8952ec.png)

I found it difficult to manage seperate files for views and controllers, and prefer the shared resource. But this method is good for translating models, specially because it's the only way I found to translate data annotation messages. 

**Use specific resource inside a View**
```
@using Microsoft.AspNetCore.Mvc.Localization
@inject IViewLocalizer Localizer

@model ContactRequest

<p>@Localizer["Learn"]</p>
<div class="form-group row">
    <label asp-for="Name" class="col-sm-3"></label>
    <div class="col-sm-8 col-xl-7">
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
</div>
```

As you see, you don't need to write anything for model and data annotation localization, the app automatically shows appropriate messages based on current selected culture.

**Use specific resource inside a Controller**
```
private readonly IStringLocalizer<HomeController> _localizer;

public IActionResult Index()
{
    ViewData["Title"] = _localizer["Home Page"];
    return View();
}
```






