using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.AddTagsToPost
{
    public class AddTagsToPostCommandHandler(IAppDbContext context) : IRequestHandler<AddTagsToPostCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddTagsToPostCommand request, CancellationToken cancellationToken)
        { 
           var post = await context.Posts.AnyAsync(s=> s.Id == request.PostId && s.UserId == request.UserId, cancellationToken);
            if (!post) return Result<bool>.NotFound();


            var tag = await context.Tags.AnyAsync(s => s.Id == request.TagId && s.UserId == request.UserId, cancellationToken);
            if(!tag) return Result<bool>.NotFound();

            var alreadyLinked = await context.PostTags.AnyAsync(s => s.TagId == request.TagId && s.PostId == request.PostId, cancellationToken);
            if(alreadyLinked) return Result<bool>.Conflict();

            var link = new PostTag
            {
                PostId = request.PostId,
                TagId = request.TagId,
                UserId = request.UserId,
            };
            context.PostTags.Add(link);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
