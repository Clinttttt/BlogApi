using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.UpdateComment
{
    public record UpdateCommentCommand(int Id, string? Content,Guid UserId) : IRequest<Result<int>>;
   
}
