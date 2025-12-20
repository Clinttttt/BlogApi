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

namespace BlogApi.Application.Queries.GetPostPaged
{
    public class GetPostPagedQueryHandler(IAppDbContext context) : IRequestHandler<GetPostPagedQuery, Result<PagedResult<PostDto>>>
    {
        public async Task<Result<PagedResult<PostDto>>> Handle(GetPostPagedQuery request, CancellationToken cancellationToken)
        {
            var query = context.Posts
                .AsNoTracking()
                .Where(s => s.Status == Domain.Enum.EntityEnum.Status.Published);
               
            var totalcount = await query.CountAsync();

            var post = await query
                .Include(s=> s.PostTags)
                .ThenInclude(s=> s.tag)
                .Include(s=> s.Category)
                .OrderByDescending(s => s.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

           if(post is null)
                return Result<PagedResult<PostDto>>.NotFound();

            var postdto = post.Select
                  (s => new PostDto
                  {
                      Title = s.Title,
                      Content = s.Content,
                      CreatedAt = s.CreatedAt,
                      CategoryName = s.Category?.Name,
                      tags = s.PostTags.Select(s => new TagDto
                      {
                          Id = s.TagId,
                          Name = s.tag?.Name
                      }).ToList()
                  }).ToList();

            var result = new PagedResult<PostDto>
            {
                Items = postdto,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalcount,

            };
            return Result<PagedResult<PostDto>>.Success(result);
        }
    }
}
