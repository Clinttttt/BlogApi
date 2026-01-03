using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class NewsletterSubscriber
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool IsActive { get; set; }
        public string? UnsubscribeToken { get; set; }
    }
}
