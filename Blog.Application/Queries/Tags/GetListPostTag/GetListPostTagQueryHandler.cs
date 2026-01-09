using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Queries.Tags.GetListPostTag
{
    public class GetListPostTagQueryHandler(ITagRespository repository) : IRequestHandler<GetListPostTagQuery, Result<List<TagDto>>>
    {
   
        public async Task<Result<List<TagDto>>> Handle(GetListPostTagQuery request,CancellationToken cancellationToken)
        {        
        
            var tags = await repository.GetListing(
                filter: c => c.PostTags.Any(s => s.TagId == c.Id && s.post!.Status == Status.Published),
                cancellationToken: cancellationToken);
            
            if (!tags.Any())
            {
                var noContent = Result<List<TagDto>>.NoContent();              
                return noContent;
            }    
            var resultList = tags.Select(s => new TagDto
            {
                Id = s.Id,
                Name = s.Name,
                TagCount = s.PostTags.Count(),
            })
            .OrderByDescending(s => s.TagCount)
            .ToList();

            var result = Result<List<TagDto>>.Success(resultList);                 
            return result;
        }
    }
}
