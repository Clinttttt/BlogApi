using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IAppDbContext context) : IRequestHandler<DeleteCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var  category = await context.Categories.FirstOrDefaultAsync(s=> s.Id == request.categoryId && s.UserId == request.UserId, cancellationToken);
            if (category is null)
                return Result<bool>.NotFound();
            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
