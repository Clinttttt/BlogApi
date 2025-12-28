using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.Featured.AddFeatured
{
    public class AddFeaturedCommandHandler(IAppDbContext context) : IRequestHandler<AddFeaturedCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddFeaturedCommand request, CancellationToken cancellationToken)
        {
            var featured = await context.Featureds.FirstOrDefaultAsync(s => s.PostId == request.PostId);
            if (featured is not null)
            {
                featured.PostId = request.PostId;
                context.Featureds.Update(featured);
            }
            else
            {
                context.Featureds.Add(new Domain.Entities.Featured
                {
                    PostId = request.PostId,
                    UserID = request.UserId
                });
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
