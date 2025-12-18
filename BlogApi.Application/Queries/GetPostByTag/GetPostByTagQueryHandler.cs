using AutoMapper;
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

namespace BlogApi.Application.Queries.GetPostByTag
{
    public class GetPostByTagQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<GetPostByTagQuery, Result<List<PostDto>>>
    {
        public async Task<Result<List<PostDto>>> Handle(GetPostByTagQuery request, CancellationToken cancellationToken)
        {
           
           var post = await context.Posts
                .AsNoTracking()
                .Include(s=> s.PostTags)
                .ThenInclude(s=> s.tag)
                .Where(s=> s.PostTags.Any(s=> s.tag != null && s.tag.Id == request.Id)).ToListAsync(cancellationToken);
            var postdto = mapper.Map<List<PostDto>>(post);
            return Result<List<PostDto>>.Success(postdto);
        }
    }
}
