using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result<int>>
    {
        private readonly IAppDbContext _context;
        public UpdateCommentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(s=> s.Id == request.Id && s.UserId == request.UserId, cancellationToken);
        
            if (comment is null)
                return Result<int>.NotFound();

            var update = new Comment
            {       
                Content = request.Content
            };
            _context.Comments.Update(update);
            await _context.SaveChangesAsync();
            return Result<int>.Success(comment.Id);
        }
    }
}
