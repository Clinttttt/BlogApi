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
    public class GetAllCategoryQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<GetAllCategoryQuery, Result<List<CategoryDto>>>
    {
        public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                 .AsNoTracking()
                 .Where(s => s.UserId == request.UserId)
                 .ToListAsync(cancellationToken);
            if(category is null) 
                    return Result<List<CategoryDto>>.NotFound();

            var categdto = mapper.Map<List<CategoryDto>>(category);
            return Result<List<CategoryDto>>.Success(categdto);

        }
    }
}
