using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Auth.Login
{
    public class GoogleLoginCommandHandler(IAuthService authService) : IRequestHandler<GoogleLoginCommand, Result<TokenResponseDto>>
    {
        public Task<Result<TokenResponseDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            return authService.GoogleLogin(request.IdToken);
        }
    }
}
