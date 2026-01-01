using AutoMapper;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetByCategory
{
    public class GetListByCategoryQueryHandler(IPostRespository respository) : IRequestHandler<GetListByCategoryQuery, Result<List<PostDto>>>
    {
        public async Task<Result<List<PostDto>>> Handle(GetListByCategoryQuery request, CancellationToken cancellationToken)
        {
            var postpage = await respository.GetNonPaginatedPostAsync(       
                filter: p => p.CategoryId == request.CategoryId,
                cancellationToken);

            if (!postpage.Any())
                return Result<List<PostDto>>.NotFound();

            var filter = postpage.Select(s => new PostDto
            {
                Id = s.Id,
                Title = s.Title,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                CategoryName = s.Category.Name,
                Status = s.Status,
                readingDuration = s.readingDuration,
                Tags = s.PostTags.Select(s => new TagDto
                {
                    Id = s.tag!.Id,
                    Name = s.tag.Name
                }).ToList(),
                IsBookMark = s.BookMarks.Any(s => s.UserId == s.UserId)
            }).ToList();
            return Result<List<PostDto>>.Success(filter);
        }
    }
}