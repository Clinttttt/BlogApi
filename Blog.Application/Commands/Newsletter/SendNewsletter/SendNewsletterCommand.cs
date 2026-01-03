using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Newsletter.SendNewsletter
{
    public record SendNewsletterCommand(string Subject, string Body) : IRequest<Result<bool>>;
  
}
