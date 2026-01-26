using Microsoft.AspNetCore.Http;

namespace BlogApi.Client.Security
{
    public static class CookieHelper
    {
        public static CookieOptions BuildCookieOptions(HttpContext context,  int days = 1)
        {
            var isHttps = context.Request.IsHttps;

            return new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps, // ✅ HTTPS = true, HTTP (Docker/dev) = false
                SameSite = isHttps
                    ? SameSiteMode.Strict
                    : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(days)
            };
        }
    }
}
