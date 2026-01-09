using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Commands.Posts.ApprovePost
{
    public class ApprovePostCommandHandler(IAppDbContext context): IRequestHandler<ApprovePostCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ApprovePostCommand request,CancellationToken cancellationToken)
        {        
            var post = await context.Posts
                .Include(s => s.User)
                .Where(s => s.User.Role == "Author" && s.Id == request.PostId)
                .FirstOrDefaultAsync(cancellationToken);

            if (post is null)
                return Result<bool>.NotFound();
         
            post.Status = Status.Published;
            await context.SaveChangesAsync(cancellationToken);
      
           
            return Result<bool>.Success(true);
        }
    }
}
