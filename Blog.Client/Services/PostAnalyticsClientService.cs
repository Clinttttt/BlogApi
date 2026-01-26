using Blog.Application.Queries.Posts.GetApprovalTotal;
using Blog.Application.Queries.Posts.RecentActivity;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Services
{
    public class PostAnalyticsClientService(HttpClient httpClient) : HandleResponse(httpClient), IPostAnalyticsClientService
    {
        public async Task<Result<StatisticsDto>> GetPublicStatistics()
            => await GetAsync<StatisticsDto>("api/PostAnalytics/GetPublicStatistics");

        public async Task<Result<StatisticsDto>> GetStatistics()
            => await GetAsync<StatisticsDto>("api/PostAnalytics/GetStatistics");
        public async Task<Result<UnreadDto>> GetAuthorUnreadTotal()
         => await GetAsync<UnreadDto>("api/PostAnalytics/GetAuthorUnreadTotal");
        public async Task<Result<UnreadDto>> GetAdminUnreadTotal()
         => await GetAsync<UnreadDto>("api/PostAnalytics/GetAdminUnreadTotal");

        public async Task<Result<UserProfileStatsDto>> GetUserProfileStats(int limit = 5, int daysBack = 7)
            => await GetAsync<UserProfileStatsDto>($"api/PostAnalytics/GetUserProfileStats?limit={limit}&daysBack={daysBack}");
    }
}