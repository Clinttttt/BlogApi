using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class ExternalLogin
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Provider { get; set; } = string.Empty; 
        public string ProviderId { get; set; } = string.Empty; 
        public DateTime LinkedAt { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public byte[]? ProfilePhotoBytes { get; set; }
        public User User { get; set; } = null!;
    }
}
