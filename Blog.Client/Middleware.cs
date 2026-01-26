using BlogApi.Client.Security;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Client.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if this is a cookie-setting request
            if (context.Request.Path == "/_auth/setcookie")
            {
                var accessToken = context.Request.Query["access_token"].ToString();
                var refreshToken = context.Request.Query["refresh_token"].ToString();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Response.Cookies.Append("AccessToken", accessToken, CookieHelper.BuildCookieOptions(context));


                }

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    context.Response.Cookies.Append("RefreshToken", refreshToken, CookieHelper.BuildCookieOptions(context, days: 7));



                }

                // Redirect to home after setting cookies
                context.Response.Redirect("/");
                return;
            }
            // Check if this is a logout cookie-clear request
            else if (context.Request.Path == "/_auth/clearcookie")
            {
                context.Response.Cookies.Delete("AccessToken");
                context.Response.Cookies.Delete("RefreshToken");
                context.Response.Redirect("/login");
                return;
            }

            await _next(context);
        }
    }

    public static class AuthCookieMiddlewareExtensions
    {
        public static IApplicationBuilder Middleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}