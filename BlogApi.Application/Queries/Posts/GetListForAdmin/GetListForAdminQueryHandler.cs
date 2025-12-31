using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetListForAdmin
{
    public class GetListForAdminQueryHandler(IPostRespository respository) : IRequestHandler<GetListForAdminQuery, Result<PagedResult<PostDto>>>
    {
        public async Task<Result<PagedResult<PostDto>>> Handle(GetListForAdminQuery request, CancellationToken cancellationToken)
        {
            var postpage = await respository.GetPaginatedPostAsync(
                request.PageNumber,
                request.PageSize,
                filter: s => s.UserId == request.UserId,
                cancellationToken);

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
                      Status = s.Status,
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
