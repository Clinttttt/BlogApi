using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Queries.Tags.GetListPostTag
{
    public class GetListPostTagQueryHandler(ITagRespository respository) : IRequestHandler<GetListPostTagQuery, Result<List<TagDto>>>
    {
        public async Task<Result<List<TagDto>>> Handle(GetListPostTagQuery request, CancellationToken cancellationToken)
        {
            var tag = await respository.GetListing(
                filter: c => c.PostTags.Any(s => s.TagId == c.Id  && s.post!.Status == Status.Published), cancellationToken);

            if (!tag.Any())
                return Result<List<TagDto>>.NoContent();

            var filter = tag.Select(s => new TagDto
            {
                Id = s.Id,
                Name = s.Name,
                TagCount = s.PostTags.Count(),
            }).OrderByDescending(s=> s.TagCount)
              .ToList();

            return Result<List<TagDto>>.Success(filter);
        }
    }
}
