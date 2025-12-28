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

namespace BlogApi.Application.Queries.Posts.GetFeatured
{
    public class GetFeaturedQueryHandler(IAppDbContext context) : IRequestHandler<GetFeaturedQuery, Result<FeaturedPostDto>>
    {
        public async Task<Result<FeaturedPostDto>> Handle(GetFeaturedQuery request, CancellationToken cancellationToken)
        {
            var featured = await context.Featureds
                .Include(s=> s.Post)
                .AsNoTracking()
                .Select(s => new FeaturedPostDto
                {
                    Title = s.Post!.Title,
                    Content = s.Post!.Content,
                    PostId = s.PostId,
                    CreatedAt = s.Post.CreatedAt,
                    ReadingDuration = s.Post.readingDuration,
                })
                .FirstOrDefaultAsync();
            if (featured is null)
                return Result<FeaturedPostDto>.NoContent();
            return Result<FeaturedPostDto>.Success(featured);
        }
    }
}
