using Blog.Application.Common.Interfaces;
using Blog.Application.Helper;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
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
    public class GetUserProfileStatsQueryHandler(IAppDbContext _context)
        : IRequestHandler<GetUserProfileStatsQuery, Result<UserProfileStatsDto>>
    {
        public async Task<Result<UserProfileStatsDto>> Handle(
            GetUserProfileStatsQuery request,
            CancellationToken cancellationToken)
        {

            var cutoffDate = DateTime.UtcNow.AddDays(-request.DaysBack);
            var activities = new List<RecentActivity>();

            IQueryable<Post> entity = _context.Posts.AsNoTracking();

            var entity1 = await entity
                .Where(p => p.CreatedAt >= cutoffDate && p.Status == Status.Published)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new RecentActivity
                {
                    Type = EnumHelper.GetTypeActivity(ActivityType.PostPublished),
                    Title = p.Title,
                    Timestamp = p.CreatedAt,
                    Icon = EnumHelper.GetIconLabel(IconLabel.Plus),
                }).ToListAsync(cancellationToken);
            activities.AddRange(entity1);


            var entity2 = await _context.Categories
                .Where(c => c.CreatedAt >= cutoffDate)
                .OrderByDescending(c => c.CreatedAt)
                .Select(p => new RecentActivity
                {
                    Type = EnumHelper.GetTypeActivity(ActivityType.CategoryAdded),
                    Title = p.Name,
                    Timestamp = p.CreatedAt,
                    Icon = EnumHelper.GetIconLabel(IconLabel.Tag),
                }).ToListAsync(cancellationToken);
            activities.AddRange(entity2);


            var entity3 = await _context.BookMarks
                .Where(b => b.CreatedAt >= cutoffDate)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new RecentActivity
                {
                    Type = EnumHelper.GetTypeActivity(ActivityType.BookmarkAdded),
                    Title = b.Post.Title,
                    Timestamp = b.CreatedAt,
                    Icon = EnumHelper.GetIconLabel(IconLabel.bookmark),
                })
                .ToListAsync(cancellationToken);
            activities.AddRange(entity3);

            var result = activities
                .OrderByDescending(a => a.Timestamp)
                .Take(request.Limit)
                .ToList();


            var userStats = await _context.Posts
       .AsNoTracking()
       .Where(p => p.UserId == request.UserId && p.Status == Status.Published)
       .GroupBy(p => p.UserId)
       .Select(g => new
       {
           PostCount = g.Count(),
           ViewsCount = g.Sum(p => p.ViewCount),
           JoinedAt = g
               .Select(p => p.User.ExternalLogins
                   .OrderBy(e => e.LinkedAt)
                   .Select(e => e.LinkedAt)
                   .FirstOrDefault())
               .FirstOrDefault()
       })
       .FirstOrDefaultAsync(cancellationToken);

            var finalResult = new UserProfileStatsDto
            {
                PostCount = userStats?.PostCount ?? 0,
                ViewsCount = userStats?.ViewsCount ?? 0,
                CreatedAt = userStats != null ? userStats.JoinedAt : DateTime.MinValue,
                Recents = result
            };
            var cachedResult = Result<UserProfileStatsDto>.Success(finalResult);
            return cachedResult;

        }
    }
}
