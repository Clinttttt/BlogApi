using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Auth.Register
{
    public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var filter = new UserDto
            {
                UserName = request.username, 
                Password = request.password
            };
            return await authService.Register(filter);
        }
    }
}
