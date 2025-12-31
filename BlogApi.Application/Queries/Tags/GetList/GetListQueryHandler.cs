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

namespace BlogApi.Application.Queries.Tags.GetAllTags
{
    public class GetListQueryHandler(IAppDbContext context) : IRequestHandler<GetListQuery, Result<List<TagDto>>>
    {
        public async Task<Result<List<TagDto>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var tag = await context.Tags
                  .Include(s=> s.PostTags)
                  .AsNoTracking()
                  .OrderByDescending(s => s.PostTags.Count)
                  .Take(6)
                  .Select(s => new TagDto
                  {
                     Id = s.Id,
                     Name = s.Name                   
                  })               
                  .ToListAsync(cancellationToken);
            if (tag is null)
                return Result<List<TagDto>>.NotFound();
          
            return Result<List<TagDto>>.Success(tag);
        }
    }
}
