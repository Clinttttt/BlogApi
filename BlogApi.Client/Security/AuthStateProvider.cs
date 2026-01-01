using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace BlogApi.Client.Security
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private bool _isInitialized;

        public AuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string?>("localStorage.getItem", "AccessToken");
                if (string.IsNullOrWhiteSpace(token))
                    return new AuthenticationState(AuthHelpers.CreateAnonymousUser());

                var user = JwtHelper.ParseToken(token);
                if (user is null)
                {
                    await AuthHelpers.ClearAuthDataAsync(_js);
                    return new AuthenticationState(AuthHelpers.CreateAnonymousUser());
                }

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(AuthHelpers.CreateAnonymousUser());
            }
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            var state = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        public async Task MarkUserAsAuthenticated() =>
            NotifyAuthenticationStateChanged(Task.FromResult(await GetAuthenticationStateAsync()));

        public async Task MarkUserAsLoggedOut()
        {
            await AuthHelpers.ClearAuthDataAsync(_js);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(AuthHelpers.CreateAnonymousUser())));
        }

        public async Task<string?> GetUserIdAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.Identity?.IsAuthenticated == true
                ? state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : null;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.Identity?.IsAuthenticated == true
                ? state.User.FindFirst(ClaimTypes.Name)?.Value
                : null;
        }
    }
}
