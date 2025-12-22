using AutoMapper;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetRecentPost
{
    public class GetRecentPostHandler(IAppDbContext context,IMapper mapper ) : IRequestHandler<GetRecentPostQuery, Result<List<PostDto>>>
    {
        public async Task<Result<List<PostDto>>> Handle(GetRecentPostQuery request, CancellationToken cancellationToken)
        {
           var query = await context.Posts
                .AsNoTracking()
                .Where(s=> s.Status == Domain.Enum.EntityEnum.Status.Published)
                .OrderByDescending(s=> s.CreatedAt)
                .Take(request.Limit)
                .ToListAsync();

            if (!query.Any())
                return Result<List<PostDto>>.NotFound();
            var querydto = mapper.Map<List<PostDto>>(query);
            return Result<List<PostDto>>.Success(querydto);
        }
    }
}
