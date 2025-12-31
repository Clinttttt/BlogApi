using AutoMapper;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPostPaged
{
    public class GetListPublishedQueryHandler(IPostRespository respository) : IRequestHandler<GetListPublishedQuery, Result<PagedResult<PostDto>>>
    {
        public async Task<Result<PagedResult<PostDto>>> Handle(GetListPublishedQuery request, CancellationToken cancellationToken)
        {

            var postpage = await respository.GetPaginatedPostAsync(
                request.PageNumber,
                request.PageSize,
                filter: p => p.Status == EntityEnum.Status.Published,
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



