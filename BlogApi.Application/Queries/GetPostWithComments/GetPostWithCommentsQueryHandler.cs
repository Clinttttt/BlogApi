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

namespace BlogApi.Application.Queries.GetPostWithComments
{
    public class GetPostWithCommentsQueryHandler : IRequestHandler<GetPostWithCommentsQuery, Result<PostWithCommentsDto>>
    {
        private readonly IAppDbContext _context;
        public GetPostWithCommentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<PostWithCommentsDto>> Handle(GetPostWithCommentsQuery request, CancellationToken cancellationToken)
        {
            var postwithcomment = await _context.Posts
                .Include(s=> s.Category)
                .Include(s=> s.PostTags)
                .ThenInclude(s=> s.tag)
                 .AsNoTracking()
                 .Where(s => s.Id == request.PostId)
                 .Select(s => new PostWithCommentsDto
                 {
                     Title = s.Title,
                     Content = s.Content,
                     PostCreatedAt = s.CreatedAt,
                     CategoryName = s.Category.Name,
                     Tags = s.PostTags.Select(s => new TagDto
                     {
                         Id = s.TagId,
                         Name = s.tag != null ? s.tag.Name : null,

                     }).ToList(),
                     Comments = s.Comments
                     .Select(c => new CommentDto
                     {
                         PostId = c.Id,
                         Content = c.Content,
                         CreatedAt = c.CreatedAt

                     }).ToList()
                 }).FirstOrDefaultAsync(cancellationToken);
            if (postwithcomment is null)
                return Result<PostWithCommentsDto>.NotFound();
            return Result<PostWithCommentsDto>.Success(postwithcomment);
        }
    }
}
