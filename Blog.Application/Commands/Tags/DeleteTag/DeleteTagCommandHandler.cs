using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Tags.DeleteTag
{
    public class DeleteTagCommandHandler(IAppDbContext context) : IRequestHandler<DeleteTagCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await context.Tags.FirstOrDefaultAsync(s => s.Id == request.TagId && s.UserId == request.UserId,cancellationToken);
            if (tag is null)
                return Result<bool>.NotFound();
            context.Tags.Remove(tag);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
