using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApi.Client.Security
{
    public static class AuthHelpers
    {
        public static ClaimsPrincipal CreateAnonymousUser() =>
            new ClaimsPrincipal(new ClaimsIdentity());

        public static async Task ClearAuthDataAsync(IJSRuntime js)
        {
            try
            {
                await js.InvokeVoidAsync("localStorage.removeItem", "AccessToken");
                await js.InvokeVoidAsync("localStorage.removeItem", "RefreshToken");
            }
            catch
            {
             
            }
        }
    }
}
