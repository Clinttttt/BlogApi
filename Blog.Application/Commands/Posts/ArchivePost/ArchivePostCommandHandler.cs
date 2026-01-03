using BlogApi.Domain.Common;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.ArchivePost
{
    public class ArchivePostCommandHandler(IAppDbContext context) : IRequestHandler<ArchivePostCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ArchivePostCommand request, CancellationToken cancellationToken)
        {
            var post = await context.Posts.FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId, cancellationToken);
            if (post is null)
                return Result<bool>.NotFound();
            post.Status = EntityEnum.Status.Draft;
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
