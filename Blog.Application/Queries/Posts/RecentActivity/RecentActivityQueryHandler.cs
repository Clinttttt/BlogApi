using Application.Queries.GetRecentActivity;
using Blog.Application.Queries.GetRecentActivity;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace Blog.Application.Queries.Posts.RecentActivity
{
    public class RecentActivityQueryHandler(IAppDbContext _context, IMemoryCache cache)
        : IRequestHandler<RecentActivityQuery, Result<List<RecentActivityItemDto>>>
    {
        public async Task<Result<List<RecentActivityItemDto>>> Handle(
            RecentActivityQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"recent-activity-{request.Limit}-{request.DaysBack}";  
            
            var isCached = cache.TryGetValue(cacheKey, out Result<List<RecentActivityItemDto>>? _);

            var cutoffDate = DateTime.UtcNow.AddDays(-request.DaysBack);

            return await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
             
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var activities = new List<RecentActivityItemDto>();
        
                var recentPosts = await _context.Posts
                    .Where(p => p.CreatedAt >= cutoffDate && p.Status == Status.Published)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new RecentActivityItemDto
                    {
                        Type = "Published new post",
                        Title = p.Title,
                        Timestamp = p.CreatedAt,
                        Icon = "plus"
                    })
                    .ToListAsync(cancellationToken);

                activities.AddRange(recentPosts);

             
                var newCategories = await _context.Categories
                    .Where(c => c.CreatedAt >= cutoffDate)
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new RecentActivityItemDto
                    {
                        Type = "Added new category",
                        Title = c.Name,
                        Timestamp = c.CreatedAt,
                        Icon = "tag"
                    })
                    .ToListAsync(cancellationToken);

                activities.AddRange(newCategories);

             
                var newBookmarks = await _context.BookMarks
                    .Where(b => b.CreatedAt >= cutoffDate)
                    .OrderByDescending(b => b.CreatedAt)
                    .Select(b => new RecentActivityItemDto
                    {
                        Type = "Added bookmark",
                        Title = b.Post.Title,
                        Timestamp = b.CreatedAt,
                        Icon = "bookmark"
                    })
                    .ToListAsync(cancellationToken);

                activities.AddRange(newBookmarks);

            
                var result = activities
                    .OrderByDescending(a => a.Timestamp)
                    .Take(request.Limit)
                    .ToList();
          

                if (!result.Any())
                    return Result<List<RecentActivityItemDto>>.NoContent();

                return Result<List<RecentActivityItemDto>>.Success(result);
            })!;
        }
    }
}
