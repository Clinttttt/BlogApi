using BlogApi.Application.Dtos;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<StatisticsDto> GetStatisticsAsync(
            Expression<Func<Post, bool>>? filter = null,
            CancellationToken cancellationToken = default);
    }
}
