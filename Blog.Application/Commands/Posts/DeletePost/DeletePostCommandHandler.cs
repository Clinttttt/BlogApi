using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        public DeletePostCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

      public async Task<Result<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(s=> s.Id == request.Id && s.UserId == request.UserId);
            if(post is null)        
                return Result<bool>.NotFound();         
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
