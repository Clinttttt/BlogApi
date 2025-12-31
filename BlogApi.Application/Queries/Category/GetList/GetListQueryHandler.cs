using AutoMapper;
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
    public class GetListQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<GetListQuery, Result<List<CategoryDto>>>
    {
        public async Task<Result<List<CategoryDto>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                  .Include(s => s.Posts)
                  .AsNoTracking()
                  .Select(c => new CategoryDto
                  {
                      Id  = c.Id,
                      Name = c.Name,
                      Slug = c.Slug,
                      CategoryCount = c.Posts.Where(s=> s.CategoryId == c.Id).Count(),
                      AllPost = c.Posts.Count()
                  })
                 .ToListAsync(cancellationToken);
            if(category is null) 
                    return Result<List<CategoryDto>>.NotFound();
            var categdto = mapper.Map<List<CategoryDto>>(category);
            return Result<List<CategoryDto>>.Success(categdto);

        }
    }
}
