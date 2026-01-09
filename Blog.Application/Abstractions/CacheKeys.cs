using Blog.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public static class CacheKeys
    {
     
        private const string PostsPrefix = "Posts";
        private const string PostPrefix = "Post";
        private const string FeaturedPrefix = "Featured";
        private const string StatisticsPrefix = "Statistics";
        private const string ActivityPrefix = "Activity";
        private const string ApprovalPrefix = "Approval";


        private const string CategoryPrefix = "Category";
        private const string TagPrefix = "Tags";



        public static class Expiration
        {
            public static readonly TimeSpan Short = TimeSpan.FromMinutes(5);
            public static readonly TimeSpan Medium = TimeSpan.FromMinutes(15);
            public static readonly TimeSpan Long = TimeSpan.FromHours(1);
            public static readonly TimeSpan VeryLong = TimeSpan.FromHours(24);
        }

      
        public static string GetPost(int postId, Guid? userId) =>
            $"{PostPrefix}:{postId}:User:{userId?.ToString() ?? "Anonymous"}";

        public static string GetPagedPosts(Guid? userId, int pageNumber, int pageSize, string filterType, int? tagId = null, int? categoryId = null)
        {
            var key = $"{PostsPrefix}:{filterType}:Page:{pageNumber}:Size:{pageSize}:User:{userId?.ToString() ?? "Anonymous"}";

            if (tagId.HasValue)
                key += $":Tag:{tagId}";

            if (categoryId.HasValue)
                key += $":Category:{categoryId}";

            return key;
        }
        public static string GetCategory() => $"{CategoryPrefix}:List";
        public static string GetTags() => $"{TagPrefix}:List";
        public static string GetFeaturedPosts() => $"{FeaturedPrefix}:List";    
        public static string GetPublicStatistics() => $"{StatisticsPrefix}:Public";
        public static string GetStatistics(Guid? userId) => $"{StatisticsPrefix}:User:{userId}";
        public static string GetRecentActivity(int limit, int daysBack) => $"{ActivityPrefix}:Limit:{limit}:Days:{daysBack}";
        public static string GetApprovalTotal() => $"{ApprovalPrefix}:Total";
        public static class Patterns
        {
            public static string AllPosts => PostsPrefix;
            public static string AllFeatured => FeaturedPrefix;
            public static string AllStatistics => StatisticsPrefix;
            public static string AllActivity => ActivityPrefix;
            public static string AllApproval => ApprovalPrefix;
            public static string AllCategory => CategoryPrefix;
            public static string AllTags => TagPrefix;

            public static string PostsByUser(Guid? userId) => $"{PostsPrefix}:*:User:{userId}";
            public static string PostsByTag(int tagId) => $"{PostsPrefix}:*:Tag:{tagId}";
            public static string PostsByCategory(int categoryId) => $"{PostsPrefix}:*:Category:{categoryId}";
            public static string SpecificPost(int? postId) => $"{PostPrefix}:{postId}";
        }
    }
}