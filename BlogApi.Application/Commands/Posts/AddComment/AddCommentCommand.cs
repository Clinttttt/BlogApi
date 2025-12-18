using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.AddComment
{
    public record AddCommentCommand(int PostId, string Content, Guid UserId) : IRequest<Result<int>>;
    
}
