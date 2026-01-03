using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
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
        Task<Result<TokenResponseDto>> CreateTokenResponse(User user);
        Task<User?> ValidateRefreshToken(Guid UserId, string RefreshToken);
    }
}
