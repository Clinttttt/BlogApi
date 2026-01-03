using BlogApi.Application.Common.Interfaces;
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
    public class GetListFeaturedQueryHandler(IPostRespository respository) : IRequestHandler<GetListFeaturedQuery, Result<List<FeaturedPostDto>>>
    {
        public async Task<Result<List<FeaturedPostDto>>> Handle(GetListFeaturedQuery request, CancellationToken cancellationToken)
        {
            var featured = await respository.GetNonPaginatedPostAsync(
                filter: s=> s.Featured.Any(),cancellationToken);

            if (!featured.Any())
                return Result<List<FeaturedPostDto>>.NoContent();
            var filter = featured.Select(s => new FeaturedPostDto
            {
                Title = s!.Title,
                Content = s!.Content,
                PostId = s.Id,
                CreatedAt = s.CreatedAt,
                ReadingDuration = s.readingDuration,
            }).ToList();          
            return Result<List<FeaturedPostDto>>.Success(filter);
     
        }
    }
}
