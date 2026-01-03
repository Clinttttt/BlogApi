using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.Posts.GetPostWithComments
{
    public class GetQueryHandler(IPostRespository respository) : IRequestHandler<GetQuery, Result<PostDetailDto>>
    {

        public async Task<Result<PostDetailDto>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            var post = await respository.GetAsync(request.PostId,cancellationToken);

            if (post is null)
                return Result<PostDetailDto>.NotFound();

            var filter = new PostDetailDto
            {

                IsBookMark = post.BookMarks.Any(s => s.UserId == request.UserId),
                PostLike = post.PostLikes.Count(),
                Title = post.Title,
                Content = post.Content,
                PhotoIsliked = post.PostLikes.Where(s => s.UserId == request.UserId).Any(),
                CommentCount = post.Comments.Count(),
                PostCreatedAt = post.CreatedAt,
                CategoryName = post.Category.Name,
                Photo = post.Photo,
                PhotoContent = post.PhotoContent,
                Author = post.Author,
                readingDuration = post.readingDuration,
                Tags = post.PostTags.Select(s => new TagDto
                {
                    Id = s.TagId,
                    Name = s.tag != null ? s.tag.Name : null,

                }).ToList(),
                Comments = post.Comments
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
                         : c.User.ExternalLogins
                           .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                         ? $"data:image/jpeg;base64,{Convert.ToBase64String(c.User.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                         : string.Empty,
                     }).ToList()
            };
            return Result<PostDetailDto>.Success(filter);
        }
                 
        }
    }

