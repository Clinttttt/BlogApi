using Blog.Application.Common.Interfaces;
using Blog.Infrastructure.Services;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetFeatured
{
    public class GetListFeaturedQueryHandler(IPostRespository repository, ICacheService cacheService) : IRequestHandler<GetListFeaturedQuery, Result<List<FeaturedPostDto>>>
    {


        public async Task<Result<List<FeaturedPostDto>>> Handle(GetListFeaturedQuery request, CancellationToken cancellationToken)
        {

            var cachekey = CacheKeys.GetFeaturedPosts();
            var expiration = CacheKeys.Expiration.Medium;

            var result = await cacheService.GetOrCreateAsync(cachekey, async () =>
            {

                var featured = await repository.GetNonPaginatedPostAsync(
                filter: s => s.Featured.Any(),
                cancellationToken: cancellationToken);

                return featured;
            },
            expiration, cancellationToken);

            if (!result!.Any())
                return Result<List<FeaturedPostDto>>.NoContent();

            var resultList = result?.Select(s => new FeaturedPostDto
            {
                Title = s.Title,
                Content = s.Content,
                PostId = s.Id,
                ViewCount = s.ViewCount ?? 0,
                CreatedAt = s.CreatedAt,
                ReadingDuration = s.readingDuration,
            }).ToList();

            return Result<List<FeaturedPostDto>>.Success(resultList!);
        }
    }
}
