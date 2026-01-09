using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface IPostRespository
    {
     

        Task<List<PostDto>> GetNonPaginatedPostAsync(
            Expression<Func<Post, bool>>?
            filter = null,
            CancellationToken cancellationToken = default);

        Task<Post?> GetAsync(
           int postId,
           CancellationToken
           cancellationToken = default);

        Task<Result<PagedResult<PostDto>>> GetPaginatedPostDtoAsync(Guid? UserIds, int PageNumber = 1,
            int PageSize = 10,
            Expression<Func<Post, bool>>? filter = null,
            CancellationToken cancellationToken = default);
    }
}
