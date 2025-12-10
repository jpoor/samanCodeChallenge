using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Saman.Backend.Share.shareMiddlewares
{
    public class LanguageHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var langHeader = context.Request.Headers["Accept-Language"].ToString();

            if (!string.IsNullOrWhiteSpace(langHeader))
            {
                var culture = CultureInfo.GetCultureInfo("fa");

                try { culture = CultureInfo.GetCultureInfo(langHeader); }
                catch { }

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            await _next(context);
        }
    }
}
