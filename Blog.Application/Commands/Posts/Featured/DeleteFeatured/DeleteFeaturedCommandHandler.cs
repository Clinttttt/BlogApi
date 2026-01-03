using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.Featured.DeleteFeatured
{
    public class DeleteFeaturedCommandHandler(IAppDbContext  context) : IRequestHandler<DeleteFeaturedCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteFeaturedCommand request, CancellationToken cancellationToken)
        {
            var featured = await context.Featureds
                .FirstOrDefaultAsync(s => s.PostId == request.PostId &&
                s.UserID == request.UserId);
            if (featured is null)
                return Result<bool>.NotFound();

            context.Featureds.Remove(featured);
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
