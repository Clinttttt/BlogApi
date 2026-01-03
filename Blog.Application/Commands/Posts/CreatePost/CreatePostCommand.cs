using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Commands.Posts.CreatePost
{
    public record CreatePostCommand(
        string Title, 
        string Content,
        int CategoryId, 
        Guid UserId,
        byte[]? Photo,
        string? PhotoContent,
        string? Author,
        Status Status,
        ReadingDuration readingDuration,
        IReadOnlyList<int> TagIds
        ) : IRequest<Result<int>>;
    
}
