using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.UpdatePost
{
    public class UpdatePostCommandHandler(IAppDbContext context): IRequestHandler<UpdatePostCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(UpdatePostCommand request,CancellationToken cancellationToken)
        {
          
            var post = await context.Posts.FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId,cancellationToken);

            if (post is null)
                return Result<int>.NotFound();

           
            post.Title = request.Title;
            post.Content = request.Content;
            post.Photo = request.Photo;
            post.PhotoContent = request.PhotoContent;
            post.Author = request.Author;
            post.readingDuration = request.readingDuration;

            await context.SaveChangesAsync(cancellationToken);     
            

            return Result<int>.Success(post.Id);
        }
    }
}
