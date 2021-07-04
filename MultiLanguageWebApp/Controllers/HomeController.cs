using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MultiLanguageWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public HomeController(
            IStringLocalizer<HomeController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["Home Page"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
