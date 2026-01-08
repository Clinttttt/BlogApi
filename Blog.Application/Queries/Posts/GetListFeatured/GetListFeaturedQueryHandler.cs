using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetFeatured
{
    public class GetListFeaturedQueryHandler(
        IPostRespository respository,
        IMemoryCache cache)
        : IRequestHandler<GetListFeaturedQuery, Result<List<FeaturedPostDto>>>
    {
        public async Task<Result<List<FeaturedPostDto>>> Handle(GetListFeaturedQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "featured-posts";

          
            if (cache.TryGetValue(cacheKey, out Result<List<FeaturedPostDto>>? cachedValue))
                return cachedValue;
        
            var featured = await respository.GetNonPaginatedPostAsync(
                filter: s => s.Featured.Any(),
                cancellationToken);

          
            if (!featured.Any())
                return Result<List<FeaturedPostDto>>.NoContent();

          
            var resultList = featured.Select(s => new FeaturedPostDto
            {
                Title = s!.Title,
                Content = s!.Content,
                PostId = s.Id,
                ViewCount = s.ViewCount ?? 0,
                CreatedAt = s.CreatedAt,
                ReadingDuration = s.readingDuration,
            }).ToList();

            var result = Result<List<FeaturedPostDto>>.Success(resultList);

         
            cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }

    }
}