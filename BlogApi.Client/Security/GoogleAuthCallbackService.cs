using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BlogApi.Client.Services
{
    public class GoogleAuthCallbackService
    {
        private readonly IAuthClientService _authService;
        private readonly NavigationManager _navigation;
        private readonly AuthStateProvider _authStateProvider;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly IJSRuntime _js;
        private readonly ILogger<GoogleAuthCallbackService> _logger;

        public GoogleAuthCallbackService(
            IAuthClientService authService,
            NavigationManager navigation,
            AuthStateProvider authStateProvider,
            ProtectedLocalStorage localStorage,
            IJSRuntime js,
            ILogger<GoogleAuthCallbackService> logger)
        {
            _authService = authService;
            _navigation = navigation;
            _authStateProvider = authStateProvider;
            _js = js;
            _localStorage = localStorage;
            _logger = logger;
        }

        public async Task ProcessLogin(string idToken)
        {
            try
            {
                var result = await _authService.GoogleLogin(idToken);
                if (result.IsSuccess && result.Value != null)
                {

                    var accessToken = result.Value.AccessToken!.Trim().Trim('"').Trim();
                    var refreshToken = result.Value.RefreshToken!.Trim().Trim('"').Trim();



                    // Cache and store tokens
                    AuthorizationDelegatingHandler.CacheToken(accessToken);
                    await _js.InvokeVoidAsync("localStorage.setItem", "AccessToken", accessToken);
                    await _js.InvokeVoidAsync("localStorage.setItem", "RefreshToken", refreshToken);


                    await _authStateProvider.InitializeAsync();
                    await _authStateProvider.MarkUserAsAuthenticated();

                    _navigation.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    _logger.LogWarning("Google login failed");
                    _navigation.NavigateTo("/login?error=google_login_failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Google login");
                _navigation.NavigateTo("/login?error=exception");
                throw;
            }
        }

        public async Task ProcessLogout()
        {
            try
            {

                AuthorizationDelegatingHandler.ClearCache();


                await _js.InvokeVoidAsync("localStorage.removeItem", "AccessToken");
                await _js.InvokeVoidAsync("localStorage.removeItem", "RefreshToken");


                await _authStateProvider.MarkUserAsLoggedOut();
                _navigation.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during logout");
                throw;
            }
        }
    }
}
