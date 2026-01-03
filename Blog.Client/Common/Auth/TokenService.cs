using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlogApi.Client.Common.Auth
{
    public class TokenService : ITokenService
    {
        private readonly ProtectedLocalStorage localStorage;
        private readonly IAuthClientService AuthClient;
        public TokenService(ProtectedLocalStorage localStorage, IAuthClientService AuthClient)
        {
            this.localStorage = localStorage;
            this.AuthClient = AuthClient;
        }   
        public async Task<bool> TryRefreshTokenAsync()
        {
            var RefreshTokens = await localStorage.GetAsync<string>("RefreshToken");
            var UserId = await localStorage.GetAsync<string>("UserId");
            if (!RefreshTokens.Success || !UserId.Success) return false;
            var request = new RefreshTokenDto()
            {
                UserId = Guid.Parse(UserId.Value!),
                RefreshToken = RefreshTokens.Value!
            };
            var NewToken = await AuthClient.RefreshToken(request);
            if (NewToken is null) return false;
            await localStorage.SetAsync("AccessToken", NewToken.Value!.AccessToken!);
            await localStorage.SetAsync("RefreshToken", NewToken.Value.RefreshToken!);
            return true;
        }
    }
}
