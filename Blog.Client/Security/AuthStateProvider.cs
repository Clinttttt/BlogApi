using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogApi.Client.Security
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                    return Anonymous();

                if (!httpContext.Request.Cookies.TryGetValue("AccessToken", out var token) ||
                    string.IsNullOrWhiteSpace(token))
                    return Anonymous();

                token = token.Trim().Trim('"').Trim();

                var user = JwtHelper.ParseToken(token);
                if (user == null)
                    return Anonymous();

                return Task.FromResult(new AuthenticationState(user));
            }
            catch
            {
                return Anonymous();
            }
        }

        public Task MarkUserAsAuthenticated()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public Task MarkUserAsLoggedOut()
        {
            NotifyAuthenticationStateChanged(Anonymous());
            return Task.CompletedTask;
        }

        private static Task<AuthenticationState> Anonymous() =>
            Task.FromResult(new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity())));
    }
}
