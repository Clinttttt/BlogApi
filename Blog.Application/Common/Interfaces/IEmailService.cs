using BlogApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<Result<bool>> SendNewsletterAsync(string Subject, string Body);
    }
}
