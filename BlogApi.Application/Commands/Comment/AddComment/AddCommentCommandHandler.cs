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
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<int>>
    {
        private readonly IAppDbContext _context;
        public AddCommentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
                  
           
            var post  = await _context.Posts.FindAsync(request.PostId, cancellationToken);
            var comment = new BlogApi.Domain.Entities.Comment
            {
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow.AddHours(8),
                PostId = request.PostId,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Result<int>.Success(comment.Id);
        }
    }
}
