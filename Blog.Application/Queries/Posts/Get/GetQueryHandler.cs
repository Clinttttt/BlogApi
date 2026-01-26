using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Common.Interfaces.Services;
using Blog.Infrastructure.Services;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPostWithComments
{
    public class GetQueryHandler(IPostRespository repository, ICacheService cache) : IRequestHandler<GetQuery, Result<PostDetailDto>>
    {

        public async Task<Result<PostDetailDto>> Handle(GetQuery request, CancellationToken cancellationToken)
        {

            var cachekey = CacheKeys.Patterns.SpecificPost(request.PostId);
            var expiration = CacheKeys.Expiration.Medium;


            var result = await cache.GetOrCreateAsync(cachekey, async () =>
            {
                var post = await repository.GetAsync(request.PostId, cancellationToken);

                return post;
            }, expiration, cancellationToken);

            if (result is null)
            {
                var notFound = Result<PostDetailDto>.NotFound();
                return notFound;
            }

            var dto = new PostDetailDto
            {
                PostId = result.Id,
                UserId = result.UserId,
                Status = result.Status,
                IsBookMark = result.BookMarks.Any(s => s.UserId == request.UserId),
                PostLike = result.PostLikes.Count(),
                Title = result.Title,
                Content = result.Content,
                PhotoIsliked = result.PostLikes.Any(s => s.UserId == request.UserId),
                CommentCount = result.Comments.Count(),
                PostCreatedAt = result.CreatedAt,
                CategoryName = result.Category.Name,
                CategoryId = result.CategoryId,
                Photo = result.Photo,
                ViewCount = result.ViewCount,
                BookMarkCount = result.BookMarks.Count(),
                PhotoContent = result.PhotoContent,
                Author = result.Author,
                readingDuration = result.readingDuration,
                Tags = result.PostTags.Select(s => new TagDto
                {
                    Id = s.TagId,
                    Name = s.tag?.Name
                }).ToList(),
                Comments = result.Comments.Select(c => new CommentDto
                {
                    IsLikedComment = c.CommentLikes.Any(cl => cl.UserId == request.UserId),
                    LikeCount = c.CommentLikes.Count(),
                    CommentId = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserName = c.User.UserName,
                    PhotoUrl = c.User.UserInfo?.Photo != null && c.User.UserInfo.Photo.Length > 0
                        ? $"data:{c.User.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(c.User.UserInfo.Photo)}"
                        : c.User.ExternalLogins
                            .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                        ? $"data:image/jpeg;base64,{Convert.ToBase64String(c.User.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                        : string.Empty
                }).ToList()
            };

            return Result<PostDetailDto>.Success(dto);

        }
    }
}
