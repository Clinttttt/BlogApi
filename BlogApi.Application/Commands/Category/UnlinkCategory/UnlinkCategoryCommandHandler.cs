using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category.UnlinkCategory
{
    public class UnlinkCategoryCommandHandler(IAppDbContext  context) : IRequestHandler<UnlinkCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UnlinkCategoryCommand request, CancellationToken cancellationToken)
        {
            var categorylink = await context.Posts.FirstOrDefaultAsync(s => s.CategoryId == request.CategoryId && s.UserId == request.UserId && s.Id == request.PostId,cancellationToken);
            if (categorylink is null)
                return Result<bool>.NotFound();
            categorylink.CategoryId = null;
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
