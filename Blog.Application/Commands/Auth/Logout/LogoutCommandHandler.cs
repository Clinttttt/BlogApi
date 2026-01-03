using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Auth.Logout
{
    public class LogoutCommandHandler( IAuthService authService) : IRequestHandler<LogoutCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await authService.Logout(request.UserId);
        }
    }
}
