using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.DeletePost
{
    public record DeletePostCommand(int Id,Guid UserId) : IRequest<Result<bool>>;
  
}
