using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category.LinkCategory
{
    public class LinkCategoryCommandHandler(IAppDbContext context) : IRequestHandler<LinkCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(LinkCategoryCommand request, CancellationToken cancellationToken)
        {          
            var post = await context.Posts.FirstOrDefaultAsync(s=> s.Id == request.PostId && s.UserId == request.UserId, cancellationToken);
            if (post is null)
                return Result<bool>.NotFound();
            post.CategoryId = request.CategoryId;
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
