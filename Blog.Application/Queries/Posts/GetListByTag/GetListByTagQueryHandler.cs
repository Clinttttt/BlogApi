using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetListByTag
{
    public class GetListByTagQueryHandler(IPostRespository respository) : IRequestHandler<GetListByTagQuery, Result<List<PostDto>>>
    {
        public async Task<Result<List<PostDto>>> Handle(GetListByTagQuery request, CancellationToken cancellationToken)
        {
            var postpage = await respository.GetNonPaginatedPostAsync(
                filter: p => p.PostTags.Any(s => s.TagId == request.TagId),
                cancellationToken);

            if (!postpage.Any())
                return Result<List<PostDto>>.NotFound();

            var filter = postpage.Select(s => new PostDto
            {
                Id = s.Id,
                Title = s.Title,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                Status = s.Status,
                CategoryName = s.Category.Name,
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
