using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Commands.Posts.TrackPostView
{
    public class TrackPostViewCommandHandler(IAppDbContext context,IPostHubService hubService, ICacheInvalidationService cacheInvalidation) : IRequestHandler<TrackPostViewCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            TrackPostViewCommand request,
            CancellationToken cancellationToken)
        {           
            var post = await context.Posts
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

            if (post == null)
                return Result<bool>.NotFound();
        
            var exists = await context.PostViews.AnyAsync(
                pv => pv.PostId == request.PostId && pv.UserId == request.UserId,
                cancellationToken);

            if (!exists)
            {        
                context.PostViews.Add(new PostView
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow.AddHours(8)
                });
            
                post.ViewCount = (post.ViewCount ?? 0) + 1;

                await context.SaveChangesAsync(cancellationToken);      
                await cacheInvalidation.InvalidatePostCacheAsync(request.PostId);
                await hubService.BroadcastViewCountUpdate(request.PostId, post.ViewCount ?? 0);
            }
            return Result<bool>.Success(true);
        }
    }
}
