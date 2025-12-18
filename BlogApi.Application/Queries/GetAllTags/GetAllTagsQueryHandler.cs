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

namespace BlogApi.Application.Queries.GetAllTags
{
    public class GetAllTagsQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<GetAllTagsQuery, Result<List<TagDto>>>
    {
        public async Task<Result<List<TagDto>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tag = await context.Tags
                  .AsNoTracking()
                  .Where(s => s.UserId == request.UserId)
                  .ToListAsync(cancellationToken);
            if (tag is null)
                return Result<List<TagDto>>.NotFound();
            var tagdto = mapper.Map<List<TagDto>>(tag);
            return Result<List<TagDto>>.Success(tagdto);
        }
    }
}
