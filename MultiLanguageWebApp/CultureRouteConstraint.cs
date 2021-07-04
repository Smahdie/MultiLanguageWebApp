using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace MultiLanguageWebApp
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("culture"))
            {
                return false;
            }

            var culture = values["culture"].ToString();
            var supportedCultures = new List<string> { "fa", "en" };

            return supportedCultures.Contains(culture);
        }
    }
}
