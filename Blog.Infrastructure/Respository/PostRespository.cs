using AutoMapper;
using Azure;
using Azure.Core;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Respository
{
    public class PostRespository(IAppDbContext context) : IPostRespository
    {
        public async Task<PagedResult<Post>> GetPaginatedPostAsync(int PageNumber, int PageSize, Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalcount = await query.CountAsync();
            var posts = await query
              .AsNoTracking()
              .OrderByDescending(s => s.CreatedAt)
              .Skip((PageNumber - 1) * PageSize)
              .Take(PageSize)
              .Include(s => s.PostTags)
                  .ThenInclude(s => s.tag)
              .Include(s => s.Category)
              .Include(s => s.BookMarks)
              .Include(s=> s.PostLikes)
              .Include(s=> s.Comments)
              .Include(s => s.User)
                  .ThenInclude(s => s.UserInfo)
              .ToListAsync(cancellationToken);

            return new PagedResult<Post>
            {
                Items = posts,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalCount = totalcount,
            };
        }

        public async Task<Result<PagedResult<PostDto>>> GetPaginatedPostDtoAsync(Guid? UserIds, int PageNumber = 1, int PageSize = 10, Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var T = await GetPaginatedPostAsync(PageNumber, PageSize, filter, cancellationToken);     
            var dto = T.Items.Select
                            (s => new PostDto
                            {
                                Id = s.Id,
                                Title = s.Title,
                                Content = s.Content,
                                Photo = s.Photo,
                                PhotoContent = s.PhotoContent,
                                Author = s.User?.UserInfo?.FullName != null ? s.User?.UserInfo.FullName : s.User?.UserName,
                                PhotoIsliked = UserIds != null && s.PostLikes.Any(pl => pl.UserId == UserIds),
                                CreatedAt = s.CreatedAt,
                                CategoryName = s.Category?.Name,
                                Status = s.Status,
                                ViewCount = s.ViewCount,
                                IsBookMark = s.BookMarks.Any(),
                                CommentCount = s.Comments.Count(),
                                PostLike = s.PostLikes.Count(),
                                readingDuration = s.readingDuration,
                                Tags = s.PostTags.Select(s => new TagDto
                                {
                                    Id = s.TagId,
                                    Name = s.tag?.Name
                                }).ToList()
                            }).ToList();

            return  Result<PagedResult<PostDto>>.Success(new PagedResult<PostDto>
            {
                Items = dto,
                PageNumber = T.PageNumber,
                PageSize = T.PageSize,
                TotalCount = T.TotalCount
            });        
        }






        public async Task<List<Post>> GetNonPaginatedPostAsync(Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedAt)
                .Include(s => s.PostTags)
                    .ThenInclude(s => s.tag)
                .Include(s => s.Category)
                .Include(s => s.BookMarks)
                .Include(s => s.User)
                    .ThenInclude(s => s.UserInfo)
                .ToListAsync(cancellationToken);
        }
        public async Task<Post?> GetAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await context.Posts
                   .AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.PostLikes)
                .Include(s => s.PostTags)
                    .ThenInclude(s => s.tag)
                .Include(s => s.BookMarks)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.CommentLikes)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                        .ThenInclude(u => u.UserInfo)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                        .ThenInclude(u => u.ExternalLogins)            
                .FirstOrDefaultAsync(s => s.Id == postId, cancellationToken);
        }




    }
}