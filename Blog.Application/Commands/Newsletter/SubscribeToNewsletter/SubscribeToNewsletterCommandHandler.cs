using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Newsletter.SubscribeToNewsletter
{
    public class SubscribeToNewsletterCommandHandler(IAppDbContext context) : IRequestHandler<SubscribeToNewsletterCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(SubscribeToNewsletterCommand request, CancellationToken cancellationToken)
        {
            var exists = await context.NewsletterSubscribers.AnyAsync(s => s.Email == request.Email);
            if(exists)
            {
                return Result<bool>.Conflict();
            }
            else
            {
                context.NewsletterSubscribers.Add(new Domain.Entities.NewsletterSubscriber
                {
                    Email = request.Email,
                    SubscribedAt = DateTime.UtcNow.AddHours(8),
                    UnsubscribeToken = Guid.NewGuid().ToString(),
                    IsActive = true
                });              
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
