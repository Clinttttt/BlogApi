using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPublicStatistics
{
    public class GetPublicStatisticsQueryHandler(IStatisticsRepository respository) : IRequestHandler<GetPublicStatisticsQuery, Result<StatisticsDto>>
    {
        public async Task<Result<StatisticsDto>> Handle(GetPublicStatisticsQuery request, CancellationToken cancellationToken)
        {
            var stats = await respository.GetStatisticsAsync(null, cancellationToken);

            if (stats is null)
                return Result<StatisticsDto>.NoContent();
            var filter = new StatisticsDto
            {
                TotalPosts = stats.TotalPosts,
                DraftPosts = stats.DraftPosts,
                PublishedPosts = stats.PublishedPosts,
            };
            return Result<StatisticsDto>.Success(filter);
        }
    }
}
