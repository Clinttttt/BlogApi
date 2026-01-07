using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlogApi.Client.Services
{
    public class GoogleAuthCallbackService
    {
        private readonly IAuthClientService _authService;
        private readonly NavigationManager _navigation;
        private readonly AuthStateProvider _authStateProvider;
        private readonly ILogger<GoogleAuthCallbackService> _logger;
      

        public GoogleAuthCallbackService(
            IAuthClientService authService,
            NavigationManager navigation,
            AuthStateProvider authStateProvider,
            ILogger<GoogleAuthCallbackService> logger
           )
        {
            _authService = authService;
            _navigation = navigation;
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
                    var accessToken = result.Value.AccessToken!.Trim().Trim('"').Trim();
                    var refreshToken = result.Value.RefreshToken!.Trim().Trim('"').Trim();

                                 
                    var encodedAccessToken = Uri.EscapeDataString(accessToken);
                    var encodedRefreshToken = Uri.EscapeDataString(refreshToken);

                    _navigation.NavigateTo($"/_auth/setcookie?access_token={encodedAccessToken}&refresh_token={encodedRefreshToken}", forceLoad: true);
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

        public Task ProcessLogout()
        {
            try
            {
              

                
                _navigation.NavigateTo("/_auth/clearcookie", forceLoad: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during logout");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}