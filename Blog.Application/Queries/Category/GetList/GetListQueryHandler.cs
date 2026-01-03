using AutoMapper;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Category.GetAllCategory
{
    public class GetListQueryHandler(ICategoryRespository respository, IMapper mapper) : IRequestHandler<GetListQuery, Result<List<CategoryDto>>>
    {
        public async Task<Result<List<CategoryDto>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var category = await respository.GetListing(filter: null, cancellationToken);

            if (!category.Any())
                return Result<List<CategoryDto>>.NoContent();

            var filter = category.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                CategoryCount = c.Posts.Where(s => s.CategoryId == c.Id).Count(),
                AllPost = c.Posts.Count()
            })
                 .ToList();
            if (category is null)
                return Result<List<CategoryDto>>.NotFound();

            var categdto = mapper.Map<List<CategoryDto>>(filter);
            return Result<List<CategoryDto>>.Success(categdto);

        }
    }
  
}
