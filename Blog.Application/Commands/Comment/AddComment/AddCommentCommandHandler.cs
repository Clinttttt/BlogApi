using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Comment.AddComment
{
    public class AddCommentCommandHandler(IAppDbContext context, IPostHubService hubService) : IRequestHandler<AddCommentCommand, Result<int>>
    {
      

        public async Task<Result<int>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {


            var post = await context.Posts.FindAsync(request.PostId, cancellationToken);
            var comment = new BlogApi.Domain.Entities.Comment
            {
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow.AddHours(8),
                PostId = request.PostId,
            };
            
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            await hubService.BroadcastSentComment(request.PostId, request.Content);
            return Result<int>.Success(comment.Id);
        }
    }
}