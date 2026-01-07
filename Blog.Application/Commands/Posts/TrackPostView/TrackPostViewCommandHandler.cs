using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Commands.Posts.TrackPostView
{
   
    public class TrackPostViewCommandHandler(IAppDbContext context, IPostHubService _hubService, IMemoryCache cache) : IRequestHandler<TrackPostViewCommand, Result<bool>>
    {
     
        public async Task<Result<bool>> Handle(TrackPostViewCommand request, CancellationToken cancellationToken)
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

                cache.Remove("featured-posts");
                await _hubService.BroadcastViewCountUpdate(request.PostId, post.ViewCount ?? 0);

            }

            return Result<bool>.Success(true);
        }

    }
}
