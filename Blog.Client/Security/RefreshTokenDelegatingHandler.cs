using BlogApi.Application.Dtos;
using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;

namespace Blog.Client.Security
{
    public class RefreshTokenDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<RefreshTokenDelegatingHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthClientService _authService;
        private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);

        public RefreshTokenDelegatingHandler(
            ILogger<RefreshTokenDelegatingHandler> logger,
            IHttpContextAccessor httpContextAccessor,
            IAuthClientService authService)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            AttachToken(request);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            await _refreshSemaphore.WaitAsync(cancellationToken);
            try
            {
                if (!await RefreshTokenAsync())
                    return response;

                AttachToken(request);
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                _refreshSemaphore.Release();
            }
        }

        private void AttachToken(HttpRequestMessage request)
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx != null &&
                ctx.Request.Cookies.TryGetValue("AccessToken", out var token))
            {
                token = token.Trim().Trim('"').Trim();
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<bool> RefreshTokenAsync()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null ||
                !ctx.Request.Cookies.TryGetValue("RefreshToken", out var refresh))
                return false;

            var result = await _authService.RefreshToken(
                new RefreshTokenDto { RefreshToken = refresh });

            if (!result.IsSuccess || result.Value == null)
                return false;

            ctx.Response.Cookies.Append("AccessToken", result.Value.AccessToken!, CookieHelper.BuildCookieOptions(ctx));
            ctx.Response.Cookies.Append("RefreshToken", result.Value.RefreshToken!, CookieHelper.BuildCookieOptions(ctx, days: 7));

            return true;
        }

       
    }
}
