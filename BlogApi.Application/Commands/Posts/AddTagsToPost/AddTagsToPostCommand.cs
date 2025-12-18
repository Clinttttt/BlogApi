using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.AddTagsToPost
{
    public record AddTagsToPostCommand(int PostId, int TagId, Guid UserId) : IRequest<Result<bool>>
    {

    }
}
