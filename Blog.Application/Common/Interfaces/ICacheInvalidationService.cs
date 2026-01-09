using Blog.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Common.Interfaces
{
    public interface ICacheInvalidationService
    {
        Task InvalidateCategoryCacheAsync();
        Task InvalidateTagsCacheAsync();
        Task InvalidatePostCacheAsync(int? postId, Guid? userId = null);
        Task InvalidatePostListCachesAsync();
        Task InvalidateFeaturedCacheAsync();
        Task InvalidateStatisticsCacheAsync(Guid? userId = null);
        Task InvalidateActivityCacheAsync();
        Task InvalidateApprovalCacheAsync();
        Task InvalidatePostsByTagAsync(int tagId);
        Task InvalidatePostsByCategoryAsync(int categoryId);
    }

    
    }
