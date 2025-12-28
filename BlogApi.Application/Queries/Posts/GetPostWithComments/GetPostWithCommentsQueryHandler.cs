using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPostWithComments
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
                .Include(s => s.Category)
                .Include(s => s.PostLikes)
                .Include(s => s.Comments)
                .ThenInclude(s => s.CommentLikes)
                .Include(s => s.PostTags)
                .ThenInclude(s => s.tag)
                .Include(s => s.Comments)
                .ThenInclude(s => s.User.UserInfo)
                 .Include(s => s.Comments)
                .ThenInclude(s => s.User.ExternalLogins)
                 .AsNoTracking()
                 .Where(s => s.Id == request.PostId)
                 .Select(s => new PostWithCommentsDto
                 {
                     IsBookMark = s.BookMarks.Any(s => s.UserId == request.UserId),
                     PostLike = s.PostLikes.Count(),
                     Title = s.Title,
                     Content = s.Content,
                     PhotoIsliked = s.PostLikes.Where(s => s.UserId == request.UserId).Any(),
                     CommentCount = s.Comments.Count(),
                     PostCreatedAt = s.CreatedAt,
                     CategoryName = s.Category.Name,
                     Photo = s.Photo,
                     PhotoContent = s.PhotoContent,
                     Author = s.Author,
                     readingDuration = s.readingDuration,
                     Tags = s.PostTags.Select(s => new TagDto
                     {
                         Id = s.TagId,
                         Name = s.tag != null ? s.tag.Name : null,

                     }).ToList(),
                     Comments = s.Comments
                     .Select(c => new CommentDto
                     {
                         IsLikedComment = c.CommentLikes.Any(cl => cl.UserId == request.UserId),
                         LikeCount = c.CommentLikes.Count(),
                         CommentId = c.Id,
                         PostId = c.PostId,
                         Content = c.Content,
                         CreatedAt = c.CreatedAt,
                         UserName = c.User.UserName,
                         PhotoUrl = c.User.UserInfo != null && c.User.UserInfo.Photo != null && c.User.UserInfo.Photo.Length > 0
                    ? $"data:{c.User.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(c.User.UserInfo.Photo)}"
                    : c.User.ExternalLogins.Select(x => x.ProfilePhotoUrl).FirstOrDefault(),

                     }).ToList()
                 }).FirstOrDefaultAsync(cancellationToken);
            if (postwithcomment is null)
                return Result<PostWithCommentsDto>.NotFound();
            return Result<PostWithCommentsDto>.Success(postwithcomment);
        }
    }
}
