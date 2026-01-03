using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> Register(UserDto request);
        Task<Result<TokenResponseDto>> Login(UserDto request);
        Task<Result<TokenResponseDto>> GoogleLogin(string idToken);
        Task<Result<TokenResponseDto>> RefreshToken(RefreshTokenDto request);
        Task<Result<bool>> Logout(Guid Id);
    }
}
