using AutoMapper;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMapper = AutoMapper.IMapper;

namespace BlogApi.Application.Queries.GetAllPosts
{

    public class GetAllPostsQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<GetAllPostsQuery, Result<List<PostDto>>>
    {
       
        public async Task<Result<List<PostDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {

            var post  = await context.Posts
                .Include(s=> s.Category)
                 .AsNoTracking()
                 .Where(s=> s.UserId ==  request.UserId)
                 .ToListAsync();
            if (post is null)
                return Result<List<PostDto>>.NotFound();

            if (!string.IsNullOrEmpty(request.Query))
            {
                var query = request.Query.ToLower();
                post = post.Where(s=> s.Title!.ToLower().Contains(query)).ToList();
            }
            post.Select(s => new PostDto
            {
                Title = s.Title,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                CategoryName = s.Category.Name,
                tags = s.PostTags.Select(s => new TagDto
                {
                    Id = s.Id,
                    Name = s.tag != null ? s.tag.Name : null,

                }).ToList()
            }).ToList();
            var postdto = mapper.Map<List<PostDto>>(post);
            return Result<List<PostDto>>.Success(postdto);
        }
    }
}
