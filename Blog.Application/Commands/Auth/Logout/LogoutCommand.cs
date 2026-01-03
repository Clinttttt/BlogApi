using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Auth.Logout
{
    public record  LogoutCommand(Guid UserId) : IRequest<Result<bool>>;
   
}
