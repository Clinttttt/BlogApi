using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetDraft
{
    public class GetDraftQueryHandler(IPostRespository respository) : IRequestHandler<GetDraftQuery, Result<PagedResult<PostDto>>>
    {
        public async Task<Result<PagedResult<PostDto>>> Handle(GetDraftQuery request, CancellationToken cancellationToken)
        {
            var postpage = await respository.GetPaginatedPostAsync(
               request.PageNumber,
               request.PageSize,
               filter: p => p.Status == EntityEnum.Status.Draft,
               cancellationToken
               );

            if (!postpage.Items.Any())
                return Result<PagedResult<PostDto>>.NotFound();

            var postdto = postpage.Items.Select
                  (s => new PostDto
                  {
                      Title = s.Title,
                      Content = s.Content,
                      CreatedAt = s.CreatedAt,
                      CategoryName = s.Category?.Name,
                      readingDuration = s.readingDuration,
                      Tags = s.PostTags.Select(s => new TagDto
                      {
                          Id = s.TagId,
                          Name = s.tag?.Name
                      }).ToList()
                  }).ToList();

            var result = new PagedResult<PostDto>
            {
                Items = postdto,
                PageNumber = postpage.PageNumber,
                PageSize = postpage.PageSize,
                TotalCount = postpage.TotalCount
            };
            return Result<PagedResult<PostDto>>.Success(result);
        }
    }
}
