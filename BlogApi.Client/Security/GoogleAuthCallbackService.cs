using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;

namespace BlogApi.Client.Services
{
    public class GoogleAuthCallbackService
    {
        private readonly IAuthClientService _authService;
        private readonly NavigationManager _navigation;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly AuthStateProvider _authStateProvider;
        private readonly ILogger<GoogleAuthCallbackService> _logger;

        public GoogleAuthCallbackService(
            IAuthClientService authService,
            NavigationManager navigation,
            ProtectedLocalStorage localStorage,
            AuthStateProvider authStateProvider,
            ILogger<GoogleAuthCallbackService> logger)
        {
            _authService = authService;
            _navigation = navigation;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        public async Task ProcessLogin(string idToken)
        {
            try
            {
                var result = await _authService.GoogleLogin(idToken);

                if (result.IsSuccess && result.Value != null)
                {
                    AuthorizationDelegatingHandler.CacheToken(result.Value.AccessToken!);
                    await _localStorage.SetAsync("AccessToken", result.Value.AccessToken!);
                    await _localStorage.SetAsync("RefreshToken", result.Value.RefreshToken!);
                    await _authStateProvider.InitializeAsync();
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