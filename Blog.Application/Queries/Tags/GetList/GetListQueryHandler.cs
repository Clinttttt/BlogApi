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

namespace BlogApi.Application.Queries.Tags.GetAllTags
{
    public class GetListQueryHandler(ITagRespository respository) : IRequestHandler<GetListQuery, Result<List<TagDto>>>
    {
        public async Task<Result<List<TagDto>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var tag = await respository.GetListing(
                null, cancellationToken);

            if (!tag.Any())
                return Result<List<TagDto>>.NoContent();

            var filter = tag.Select(s => new TagDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();       

            return Result<List<TagDto>>.Success(filter);
        }
    }
}

