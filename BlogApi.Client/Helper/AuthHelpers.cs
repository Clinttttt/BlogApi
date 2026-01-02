using System.Security.Claims;

namespace BlogApi.Client.Security
{
    public static class AuthHelpers
    {
        public static ClaimsPrincipal CreateAnonymousUser() =>
            new ClaimsPrincipal(new ClaimsIdentity());
    }
}