using Blog.Application.Common.Interfaces;
using Blog.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Abstractions
{
    public class CacheInvalidationService : ICacheInvalidationService
    {
        private readonly ICacheService _cacheService;

        public CacheInvalidationService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task InvalidateCategoryCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllCategory);
        }

        public async Task InvalidateTagsCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllTags);
        }
        public async Task InvalidatePostCacheAsync(int? postId, Guid? userId = null)
        {        
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.SpecificPost(postId));
            await InvalidatePostListCachesAsync();
        }

        public async Task InvalidatePostListCachesAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllPosts);
        }

        public async Task InvalidateFeaturedCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllFeatured);
        }

        public async Task InvalidateStatisticsCacheAsync(Guid? userId = null)
        {
            if (userId.HasValue)
            {              
                var userStatKey = CacheKeys.GetStatistics(userId.Value);
                await _cacheService.RemoveAsync(userStatKey);
            }
     
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllStatistics);
        }

        public async Task InvalidateActivityCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllActivity);
        }

        public async Task InvalidateApprovalCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.AllApproval);
        }

        public async Task InvalidatePostsByTagAsync(int tagId)
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.PostsByTag(tagId));
        }

        public async Task InvalidatePostsByCategoryAsync(int categoryId)
        {
            await _cacheService.RemoveByPatternAsync(CacheKeys.Patterns.PostsByCategory(categoryId));
        }
    }
}
