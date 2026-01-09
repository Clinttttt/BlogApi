using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using Microsoft.Extensions.Configuration;
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
using static BlogApi.Infrastructure.Respository.PostRespository;

namespace BlogApi.Infrastructure.Respository
{
    public class PostRespository(IAppDbContext context, IMapper mapper) : IPostRespository
    {

        public async Task<Result<PagedResult<PostDto>>> GetPaginatedPostDtoAsync(
            Guid? userId,
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<Post, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync(cancellationToken);


            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PostDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            if (userId.HasValue)
            {
                var postIds = items.Select(p => p.Id).ToList();
                var likedPostIds = await context.PostLikes
                    .Where(pl => postIds.Contains(pl.PostId) && pl.UserId == userId.Value)
                    .Select(pl => pl.PostId)
                    .ToListAsync(cancellationToken);

                var likedSet = new HashSet<int?>(likedPostIds);

                foreach (var item in items)
                {
                    item.PhotoIsliked = likedSet.Contains(item.Id);
                }
            }
            foreach (var item in items)
            {
                if (item.Photo != null && !string.IsNullOrEmpty(item.PhotoContent))
                {
                    item.Preview = $"data:{item.PhotoContent};base64,{Convert.ToBase64String(item.Photo)}";
                }
            }

            return Result<PagedResult<PostDto>>.Success(new PagedResult<PostDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }
        public async Task<List<PostDto>> GetNonPaginatedPostAsync(Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = context.Posts.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .ProjectTo<PostDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                if (item.Photo != null && !string.IsNullOrEmpty(item.PhotoContent))
                {
                    item.Preview = $"data:{item.PhotoContent};base64,{Convert.ToBase64String(item.Photo)}";
                }
            }

            return items;
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
                          .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == postId, cancellationToken);
        }




    }
}