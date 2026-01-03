using AutoMapper;
using Azure.Core;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PagedResult<Post>> GetPaginatedPostAsync(int PageNumber = 1, int PageSize = 10,Expression<Func<Post, bool>>? filter = null,CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalcount = await query.CountAsync();
            var posts = await query
              .OrderByDescending(s => s.CreatedAt)
              .Skip((PageNumber - 1) * PageSize)
              .Take(PageSize)
              .Include(s => s.PostTags)
                  .ThenInclude(s => s.tag)
              .Include(s => s.Category)
              .Include(s => s.BookMarks)
              .ToListAsync(cancellationToken);

            return new PagedResult<Post>
            {
                Items = posts,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalCount = totalcount,
            };
        }
        public async Task<List<Post>> GetNonPaginatedPostAsync(Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query
                .OrderByDescending(s => s.CreatedAt)
                .Include(s => s.PostTags)
                    .ThenInclude(s => s.tag)
                .Include(s => s.Category)
                .Include(s=> s.BookMarks)
                .ToListAsync(cancellationToken);
        }
        public async Task<Post?> GetAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await context.Posts
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
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == postId, cancellationToken);
        }



    }
}