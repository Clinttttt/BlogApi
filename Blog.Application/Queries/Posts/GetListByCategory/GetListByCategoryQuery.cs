using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetByCategory
{
    public record GetListByCategoryQuery(int CategoryId) : IRequest<Result<List<PostDto>>>;
  
}
