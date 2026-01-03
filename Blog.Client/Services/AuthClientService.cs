
/*using Azure;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Json;

namespace BlogApi.Client.Services
{
    public class AuthClientService : IAuthClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localstorage;
        public AuthClientService(HttpClient httpClient, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _localstorage = localStorage;
        }
        public async Task<Result<AuthResult>> Register(UserDto user)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Auth/Register", user);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<AuthResult>();
            if (result is null)
                return Result<AuthResult>.NotFound();
            return Result<AuthResult>.Success(result);
        }
        public async Task<Result<TokenResponseDto>> GoogleLogin(string IdToken)
        {
           
            var command = new { IdToken = IdToken };

            var request = await _httpClient.PostAsJsonAsync("api/Auth/GoogleLogin", command);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (result is null)
                return Result<TokenResponseDto>.NotFound();
            return Result<TokenResponseDto>.Success(result);
        }
        public async Task<Result<TokenResponseDto>> Login(UserDto user)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Auth/Login", user);
            request.EnsureSuccessStatusCode();
            var result =  await request.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (result is null)
                return Result<TokenResponseDto>.NotFound();
            return Result<TokenResponseDto>.Success(result);
        }
       public async Task<Result<TokenResponseDto>> RefreshToken(RefreshTokenDto refreshToken)
         {
             var request = await _httpClient.PostAsJsonAsync("api/Auth/RefreshToken", refreshToken);
             request.EnsureSuccessStatusCode();
          
            var result = await request.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (result is null)
                return Result<TokenResponseDto>.NotFound();
            return Result<TokenResponseDto>.Success(result);
         }
        public async Task<bool> TryRefreshTokenAsync()
        {
            var RefreshTokens = await _localstorage.GetAsync<string>("RefreshToken");
            var UserId = await _localstorage.GetAsync<string>("UserId");
            if (!RefreshTokens.Success || !UserId.Success) return false;
            var request = new RefreshTokenDto()
            {
                UserId = Guid.Parse(UserId.Value!),
                RefreshToken = RefreshTokens.Value!
            };
            var NewToken = await RefreshToken(request);
            if (NewToken is null) return false;
            await _localstorage.SetAsync("AccessToken", NewToken.Value!.AccessToken!);
            await _localstorage.SetAsync("RefreshToken", NewToken.Value.RefreshToken!);
            return true;
        }
    }
}
*/












using Azure;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Request.Auth;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Identity.Client;
using System.Net.Http.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlogApi.Client.Services
{
    public class AuthClientService(HttpClient httpClient) : HandleResponse(httpClient), IAuthClientService
    {
        public async Task<Result<AuthResult>> Register(UserDto user) => await PostAsync<UserDto, AuthResult>("api/Auth/Register", user);
        public async Task<Result<TokenResponseDto>> GoogleLogin(string idToken) => await PostAsync<GoogleLoginRequest, TokenResponseDto>("api/Auth/GoogleLogin", new GoogleLoginRequest { IdToken = idToken });
        public async Task<Result<TokenResponseDto>> Login(UserDto user) => await PostAsync<UserDto, TokenResponseDto>("api/Auth/Login", user);
        public async Task<Result<TokenResponseDto>> RefreshToken(RefreshTokenDto refreshToken) => await PostAsync<RefreshTokenDto, TokenResponseDto>("api/Auth/RefreshToken", refreshToken);    
    }
}
