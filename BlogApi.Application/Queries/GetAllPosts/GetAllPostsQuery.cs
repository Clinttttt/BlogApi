using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.GetAllPosts
{
    public record GetAllPostsQuery(int CategoryId, Guid UserId, string Query = "" ) : IRequest<Result<List<PostDto>>>;
    
}
