
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Queries.User.Get;
using Blog.Application.Queries.User.GetListAuthor;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogApi.Infrastructure.Respository
{
    public class UserRespository(IAppDbContext context) : IUserRespository
    {



        public async Task<Result<PagedResult<AuthorStatDto>>> GetListing(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {

            var users = await context.Users
              .AsNoTracking()
              .Where(u => u.Role == "Author")
              .OrderByDescending(u => u.Posts.Count)
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
               .Select(u => new AuthorStatDto
               {
                   UserId = u.Id,
                   Name = u.UserName,
                   PostCount = u.Posts.Count,
                   CreatedAt = u.ExternalLogins
                     .OrderByDescending(l => l.LinkedAt)
                     .Select(l => l.LinkedAt)
                     .FirstOrDefault(),
                   ProfilePhoto = u.ExternalLogins
                            .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                        ? $"data:image/jpeg;base64,{Convert.ToBase64String(u.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                        : string.Empty
               }).ToListAsync(cancellationToken);

            var totalcount = users.Count();

            return Result<PagedResult<AuthorStatDto>>.Success(new PagedResult<AuthorStatDto>
            {
                Items = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalcount
            });
        }
        public async Task<List<AuthorDto>> Top5(CancellationToken cancellationToken = default)
        {
            var data = await context.Users
            .AsNoTracking()
                .Where(s => s.Role == "Author")
                .OrderByDescending(s => s.Posts.Count)
                .Select(s => new
                {
                    s.Id,
                    s.UserName,
                    PostCount = s.Posts.Count,
                    GooglePhoto = s.ExternalLogins
             .Where(el => el.Provider == "Google")
             .Select(el => el.ProfilePhotoBytes)
             .FirstOrDefault()
                })
            .Take(5)
            .ToListAsync(cancellationToken);

            return data.Select(s => new AuthorDto
            {
                UserId = s.Id,
                Name = s.UserName,
                PostCount = s.PostCount,
                ProfilePhoto =
        s.GooglePhoto != null
            ? $"data:image/jpeg;base64,{Convert.ToBase64String(s.GooglePhoto)}"
            : string.Empty
            }).ToList();


        }
        public async Task<Result<UserDashboardDto>> Get(Guid UserId, CancellationToken cancellationToken = default)
        {
            IQueryable<User> user = context.Users.AsNoTracking();

            var get = await user.Where(s => s.Id == UserId).Select(s => new
            {
                s.UserName,
                CreatedAt = s.ExternalLogins.Select(s => s.LinkedAt).First(),
                Photo = s.UserInfo != null ? s.UserInfo.Photo : null,
                PhotoContentType = s.UserInfo != null ? s.UserInfo.PhotoContentType : null,
                GooglePhoto = s.ExternalLogins.Where(s => s.Provider == "Google").Select(s => s.ProfilePhotoBytes).FirstOrDefault(),

                TotalPost = s.Posts.Count(),
                TotalViews = s.Posts.Sum(s => s.PostViews.Count()),
                TotalLikes = s.Posts.Sum(s => s.PostLikes.Count()),
                TotalBookMarks = s.Posts.Sum(s => s.BookMarks.Count()),

                Posts = s.Posts.Select(s => new PostDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    CreatedAt = s.CreatedAt,
                    Tags = s.PostTags.Select(s => new TagDto
                    {
                        Id = s.TagId,
                        Name = s.tag!.Name,
                        PostId = s.PostId,
                        TagCount = s.tag.PostTags.Count()
                    }).ToList(),
                    IsBookMark = s.BookMarks.Any(s => s.UserId == UserId),
                    Author = s.Author,
                    PhotoIsliked = s.PostLikes.Any(s => s.UserId == UserId),
                    BookMarkCount = s.BookMarks != null ? s.BookMarks.Count() : 0,
                    PostLikeCount = s.PostLikes != null ? s.PostLikes.Count() : 0,
                    ViewCount = s.ViewCount != null ? s.ViewCount : 0

                }).ToList()
            }).FirstOrDefaultAsync();

            if (get is null)
            {
                return Result<UserDashboardDto>.NoContent();
            }

            var dto = new UserDashboardDto
            {
                Name = get.UserName,
                CreatedAt = get.CreatedAt,
                ProfilePhoto = get.Photo != null && get.Photo.Length > 0 ? $"data:{get.PhotoContentType};base64,{Convert.ToBase64String(get.Photo)}"
                : get?.GooglePhoto != null
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(get.GooglePhoto)}"
                : string.Empty,
                TotalPost = get?.TotalPost,
                TotalViews = get?.TotalViews,
                TotalBookMarks = get?.TotalBookMarks,
                TotalLikes = get?.TotalLikes,

                Posts = get?.Posts != null ? get.Posts : new List<PostDto>()
            };
            return Result<UserDashboardDto>.Success(dto);
        }


        public async Task<UserProfileDto> GetUserProfileAsync(Guid? UserId, CancellationToken cancellationToken = default)
        {

            var data = await context.Users
                     .AsNoTracking()
                 .Where(s => s.Id == UserId)
              .Select(s => new
              {
                  s.Id,
                  s.UserName,
                  FullName = s.UserInfo != null ? s.UserInfo.FullName : null,
                  Photo = s.UserInfo != null ? s.UserInfo.Photo : null,
                  PhotoContentType = s.UserInfo != null ? s.UserInfo.PhotoContentType : null,
                  GooglePhoto = s.ExternalLogins
             .Where(el => el.Provider == "Google")
             .Select(el => el.ProfilePhotoBytes)
             .FirstOrDefault()
              }).FirstOrDefaultAsync();


            var dto = new UserProfileDto
            {
                UserId = data?.Id,
                Name = data?.FullName ?? data?.UserName,
                PhotoUrl =
              data?.Photo != null && data.Photo.Length > 0
            ? $"data:{data.PhotoContentType};base64,{Convert.ToBase64String(data.Photo)}"
            : data?.GooglePhoto != null
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(data.GooglePhoto)}"
                : string.Empty
            };
            return dto;

        }


    }
}
