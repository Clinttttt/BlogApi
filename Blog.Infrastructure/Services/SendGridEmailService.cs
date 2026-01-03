using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _Apikey;
        private readonly IAppDbContext _context;
        public SendGridEmailService(IConfiguration configuration, IAppDbContext context)
        {
            _Apikey = configuration["SendGrid:ApiKey"] ?? "fake-key-for-testing";
            _context = context;
        }
        public async Task<Result<bool>> SendNewsletterAsync(string Subject, string Body)
        {
            var subscribers = await _context.NewsletterSubscribers
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.Email,
                    s.UnsubscribeToken,
                })
                .ToListAsync();
            var Client = new SendGridClient(_Apikey);

            foreach (var email in subscribers)
            {
                var personalizedBody = Body.Replace("{{TOKEN}}", email.UnsubscribeToken);

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("clintvillanueva82@gmail.com", "Clint's Blog"),
                    Subject = Subject,
                    HtmlContent = personalizedBody
                };
                msg.AddTo(email.Email);
                msg.SetClickTracking(false, false);
                await Client.SendEmailAsync(msg);
            }
            return Result<bool>.Success(true);
        }
    }
}
