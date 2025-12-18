using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;

namespace BlogApi.Application.Commands.Auth.Login
{
    public record GoogleLoginCommand(string IdToken) : IRequest<Result<TokenResponseDto>>;
    
}
