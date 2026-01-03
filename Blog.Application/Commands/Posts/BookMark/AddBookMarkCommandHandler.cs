using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.BookMark
{
    public class AddBookMarkCommandHandler(IAppDbContext context) : IRequestHandler<AddBookMarkCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddBookMarkCommand request, CancellationToken cancellationToken)
        {
           var bookmark = await context.BookMarks
                .FirstOrDefaultAsync(s=> s.PostId == request.PostId && s.UserId == request.UserId, cancellationToken);
            if(bookmark is null)
            {
                context.BookMarks.Add( new Domain.Entities.BookMark
                {
                    PostId = request.PostId,
                    UserId = request.UserId
                });
            }
            else
            {
                context.BookMarks.Remove(bookmark);
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
