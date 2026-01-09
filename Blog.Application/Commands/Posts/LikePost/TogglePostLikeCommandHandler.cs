using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.LikePost
{
    public class TogglePostLikeCommandHandler(IAppDbContext context, ICacheInvalidationService cacheInvalidation) : IRequestHandler<TogglePostLikeCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(TogglePostLikeCommand request,CancellationToken cancellationToken)
        {
           
            var like = await context.PostLikes
                .FirstOrDefaultAsync(
                s => s.PostId == request.PostId && s.UserId == request.UserId,
                cancellationToken);

            if (like is null)
            {             
                context.PostLikes.Add(new PostLike
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow.AddHours(8)
                });
            }
            else
            {
                context.PostLikes.Remove(like);
            }

            await context.SaveChangesAsync(cancellationToken);
            await cacheInvalidation.InvalidatePostListCachesAsync();
            await cacheInvalidation.InvalidatePostCacheAsync(request.PostId);

            return Result<bool>.Success(true);
        }
    }
}
