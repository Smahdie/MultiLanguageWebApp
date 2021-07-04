using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiLanguageWebApp
{
    public class RouteValueRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var cultureCode = GetCultureCodeFromRequestPath(httpContext);
            if (cultureCode == null)
            {
                return Task.FromResult((ProviderCultureResult)null);
            }

            var requestCulture = new ProviderCultureResult(cultureCode);

            return Task.FromResult(requestCulture);
        }

        private string GetCultureCodeFromRequestPath(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.HasValue)
                return GetDefaultCultureCode();

            var requestPath = httpContext.Request.Path.Value;

            if (requestPath[0] == '/' && (requestPath.Length > 3 && requestPath[3] == '/') || requestPath.Length == 3)
            {
                var cultureCode = requestPath.Substring(1, 2);

                if (CheckCultureCode(cultureCode))
                    return cultureCode;

                return null;
            }

            return GetDefaultCultureCode();
        }

        private string GetDefaultCultureCode()
        {
            return Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
        }

        private bool CheckCultureCode(string cultureCode)
        {
            return Options.SupportedCultures.Select(c => c.TwoLetterISOLanguageName).Contains(cultureCode);
        }
    }
}
