using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Queries.Posts.PostDashboard
{
    public class PostDashboardQueryHandler(IAppDbContext context) : IRequestHandler<PostDashboardQuery, Result<PostDashboardDto>>
    {
        public async Task<Result<PostDashboardDto>> Handle(PostDashboardQuery request, CancellationToken cancellationToken)
        {
            var post = await context.Posts
                .Include(s => s.Category)
                .Include(s => s.PostTags)
                .ThenInclude(s => s.tag)
                .AsNoTracking()
                .Where(s => s.UserId == request.UserId)
                 .ToListAsync(cancellationToken);

            var filter = post.Select(s => new PostDashboardDto
            {
                TotalPosts = post.Count(),
                DraftPosts = post.Where(s => s.Status == Domain.Enums.EntityEnum.Status.Draft).Count(),
                PublishedPosts = post.Where(s => s.Status == Domain.Enums.EntityEnum.Status.Published).Count(),
                Posts = post.Select(s => new PostWithStatusDto
                {
                    Status = s.Status,
                    PostId = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    CreatedAt = s.CreatedAt,
                    CategoryName = s.Category.Name,
                    readingDuration = s.readingDuration,
                    tags = s.PostTags.Select(s => new TagDto
                    {
                        Id = s.tag!.Id,
                        Name = s.tag.Name,
                    }).ToList(),
                    IsBookMark = false
                }).ToList()
            }).FirstOrDefault();
            if (filter is null)
                return Result<PostDashboardDto>.NoContent();
            return Result<PostDashboardDto>.Success(filter);

        }
    }
}
