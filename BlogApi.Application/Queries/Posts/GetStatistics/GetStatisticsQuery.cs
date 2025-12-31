using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetStatistics
{
    public record GetStatisticsQuery(Guid UserId) : IRequest<Result<StatisticsDto>>;
   
}
