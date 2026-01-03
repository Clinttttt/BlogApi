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

namespace BlogApi.Application.Commands.Newsletter.UnSubscribeToNewsletter
{
    public class UnSubscribeToNewsletterCommandHandler(IAppDbContext context) : IRequestHandler<UnSubscribeToNewsletterCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UnSubscribeToNewsletterCommand request, CancellationToken cancellationToken)
        {
            var findtoken = await context.NewsletterSubscribers
                .FirstOrDefaultAsync(s => s.UnsubscribeToken == request.token);
            if (findtoken is null)
                return Result<bool>.NotFound();
            findtoken.IsActive = false;
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
