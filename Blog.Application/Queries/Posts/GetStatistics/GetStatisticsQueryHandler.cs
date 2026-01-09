using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetStatistics
{
    public class GetStatisticsQueryHandler(IStatisticsRepository respository) : IRequestHandler<GetStatisticsQuery, Result<StatisticsDto>>
    {
        public async Task<Result<StatisticsDto>> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
           
            var stats = await respository.GetStatisticsAsync(
                filter: s => s.UserId == request.UserId, cancellationToken);

            if (stats is null)
            {
                var noContent = Result<StatisticsDto>.NoContent();
              
                return noContent;
            }

            var filter = new StatisticsDto
            {
                TotalPosts = stats.TotalPosts,
                DraftPosts = stats.DraftPosts,
                PublishedPosts = stats.PublishedPosts,
            };
            var result = Result<StatisticsDto>.Success(filter);
         

            return result;
        }
    }
}
