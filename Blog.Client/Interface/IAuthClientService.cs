using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface IAuthClientService
    {
        Task<Result<TokenResponseDto>> RefreshToken(RefreshTokenDto refreshToken);
        Task<Result<AuthResult>> Register(UserDto user);
        Task<Result<TokenResponseDto>> GoogleLogin(string IdToken);
        Task<Result<TokenResponseDto>> Login(UserDto user);
    }
}
