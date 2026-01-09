using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPublicStatistics
{
    public class GetPublicStatisticsQueryHandler(
        IStatisticsRepository repository
    )
        : IRequestHandler<GetPublicStatisticsQuery, Result<StatisticsDto>>
    {
     

        public async Task<Result<StatisticsDto>> Handle(
            GetPublicStatisticsQuery request,
            CancellationToken cancellationToken)
        {          
      
       
            var stats = await repository.GetStatisticsAsync(null, cancellationToken);
           
            if (stats is null)
            {
                var noContent = Result<StatisticsDto>.NoContent();
              
                return noContent;
            }
       
            var dto = new StatisticsDto
            {
                TotalPosts = stats.TotalPosts,
                DraftPosts = stats.DraftPosts,
                PublishedPosts = stats.PublishedPosts,
            };
            
            var result = Result<StatisticsDto>.Success(dto);
         

            return result;
        }
    }
}
