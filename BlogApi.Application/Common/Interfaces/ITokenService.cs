using BlogApi.Application.Dtos;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> CreateTokenResponse(User user);
        Task<User?> ValidateRefreshToken(int UserId, string RefreshToken);
    }
}
