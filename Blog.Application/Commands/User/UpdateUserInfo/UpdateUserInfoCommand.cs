using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.User.UpdateUserInfo
{
    public record UpdateUserInfoCommand(string? Bio, string? FullName, byte[]? Photo, string? PhotoContentType, Guid UserId) : IRequest<Result<bool>>;
   
}
