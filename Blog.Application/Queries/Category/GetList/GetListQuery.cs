using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Category.GetAllCategory
{
    public record GetListQuery() : IRequest<Result<List<CategoryDto>>>;
   
}
