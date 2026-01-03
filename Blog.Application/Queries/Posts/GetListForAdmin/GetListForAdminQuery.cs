using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetListForAdmin
{
    public record GetListForAdminQuery(Guid UserId, int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<PostDto>>>;
 
}
