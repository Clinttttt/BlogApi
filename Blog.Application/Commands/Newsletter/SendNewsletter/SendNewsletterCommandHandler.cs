using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Newsletter.SendNewsletter
{
    public class SendNewsletterCommandHandler(IEmailService emailService) : IRequestHandler<SendNewsletterCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(SendNewsletterCommand request, CancellationToken cancellationToken)
        {
          return await emailService.SendNewsletterAsync(request.Subject, request.Body);       
        }
    }
}
