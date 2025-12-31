using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.BookMark.GetAllBookMark
{
    public record GetListQuery(Guid UserId) : IRequest<Result<List<PostDto>>>;
    
}
