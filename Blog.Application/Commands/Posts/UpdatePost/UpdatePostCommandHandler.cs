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

namespace BlogApi.Application.Commands.Posts.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<int>>
    {
        private readonly IAppDbContext _context;
        public UpdatePostCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
           var post = await _context.Posts.FirstOrDefaultAsync(s=> s.Id == request.Id && s.UserId == request.UserId, cancellationToken);
            if (post is null)
                return Result<int>.NotFound();
            var update = new Post
            {
                Title = request.Title,
                Content = request.Content,
                Photo = request.Photo,
                PhotoContent = request.PhotoContent,
                Author = request.Author,
                readingDuration = request.readingDuration
            };
            _context.Posts.Update(update);
            await _context.SaveChangesAsync();
            return Result<int>.Success(post.Id);
        }
    }
}
