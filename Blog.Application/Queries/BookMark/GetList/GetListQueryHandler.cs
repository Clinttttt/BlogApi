using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.BookMark.GetAllBookMark
{
    public class GetListQueryHandler(IPostRespository respository) : IRequestHandler<GetListQuery, Result<List<PostDto>>>
    {

        public async Task<Result<List<PostDto>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var bookmark = await respository.GetNonPaginatedPostAsync(
                filter: b=> b.BookMarks.Any(s=> s.UserId == request.UserId),
                cancellationToken);

            if (!bookmark.Any())
                return Result<List<PostDto>>.NoContent();
            var filter = bookmark.Select(s => new PostDto
            {
                Id = s.Id,
                IsBookMark = s.BookMarks.Any(s => s.UserId == request.UserId),
                Title = s.Title,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                CategoryName = s.Category.Name,
                readingDuration = s.readingDuration,
                Tags = s.PostTags.Select(s => new TagDto
                {
                    Id = s.TagId,
                    Name = s.tag != null ? s.tag.Name : "N/A",

                }).ToList(),
            }).ToList();
            return Result<List<PostDto>>.Success(filter);
        }
    }
}
