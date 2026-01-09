using AutoMapper;
using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Queries.Category.GetListPostCategory
{
    public class GetListPostCategoryQueryHandler(ICategoryRespository respository): IRequestHandler<GetListPostCategoryQuery, Result<List<CategoryDto>>>
    {
        public async Task<Result<List<CategoryDto>>> Handle(
            GetListPostCategoryQuery request,
            CancellationToken cancellationToken)
        {
           

            var categories = await respository.GetListing(filter: c => c.Posts.Any(p => p.Status == Status.Published),cancellationToken);
            if (!categories.Any())
                return Result<List<CategoryDto>>.NoContent();

            var result = categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    CategoryCount = c.Posts.Count(p => p.Status == Status.Published),
                    AllPost = c.Posts.Count()
                })
                .OrderByDescending(c => c.CategoryCount)
                .Take(6)
                .ToList();

            var finalResult = Result<List<CategoryDto>>.Success(result);
           
            return finalResult;
        }
    }
}