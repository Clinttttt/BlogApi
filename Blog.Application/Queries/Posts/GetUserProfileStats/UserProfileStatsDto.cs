using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;

namespace Blog.Application.Queries.Posts.RecentActivity
{
    public class UserProfileStatsDto
    {
        public int? PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ViewsCount { get; set; }
        public List<RecentActivity>? Recents { get; set; }
   
    }
    public class RecentActivity
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Icon { get; set; }
        public string TimeAgo => CalculateTimeAgo(Timestamp);

        private string CalculateTimeAgo(DateTime timestamp)
        {
            var span = DateTime.UtcNow - timestamp;

            if (span.TotalMinutes < 1)
                return "just now";

            if (span.TotalHours < 1)
                return $"{(int)span.TotalMinutes} minute{((int)span.TotalMinutes != 1 ? "s" : "")} ago";

            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hour{((int)span.TotalHours != 1 ? "s" : "")} ago";

            if (span.TotalDays < 30)
                return $"{(int)span.TotalDays} day{((int)span.TotalDays != 1 ? "s" : "")} ago";

            if (span.TotalDays < 365)
                return $"{(int)(span.TotalDays / 30)} month{((int)(span.TotalDays / 30) != 1 ? "s" : "")} ago";

            return $"{(int)(span.TotalDays / 365)} year{((int)(span.TotalDays / 365) != 1 ? "s" : "")} ago";
        }
    }
}