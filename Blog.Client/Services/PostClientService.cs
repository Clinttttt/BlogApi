using Application.Queries.GetRecentActivity;
using Blog.Application.Queries.GetRecentActivity;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Request.Posts;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Services
{
    public class PostClientService(HttpClient httpClient) : HandleResponse(httpClient), IPostClientService
    {
        public async Task<Result<int>> Create(CreatePostRequest dto)
            => await PostAsync<CreatePostRequest, int>("api/Posts/AddPost", dto);

        public async Task<Result<PostDetailDto>> Get(GetPostRequest request)
            => await GetAsync<PostDetailDto>($"api/Posts/GetPost?PostId={request.PostId}");

        public async Task<Result<int>> Update(UpdatePostRequest dto)
            => await UpdateAsync<UpdatePostRequest, int>("api/Posts/UpdatePost", dto);

        public async Task<Result<bool>> Archive(ArchivePostRequest request)
            => await UpdateAsync<bool>($"api/Posts/ArchivedPost?PostId={request.PostId}");

        public async Task<Result<bool>> Delete(DeletePostRequest request)
            => await DeleteAsync<bool>($"api/Posts/DeletePost?PostId={request.PostId}");

    


        public async Task<Result<PagedResult<PostDto>>> ListPublishedForAdmin(ListPaginatedRequest request)
           => await GetAsync<PagedResult<PostDto>>(
               $"api/Posts/ListPublishedForAdmin?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<PagedResult<PostDto>>> ListDraftForAdmin(ListPaginatedRequest request)
           => await GetAsync<PagedResult<PostDto>>(
               $"api/Posts/ListDraftForAdmin?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<PagedResult<PostDto>>> ListPublished(ListPaginatedRequest request)
            => await GetAsync<PagedResult<PostDto>>(
                $"api/Posts/ListPublished?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<PagedResult<PostDto>>> ListByTag(int tagId, ListPaginatedRequest request)
            => await GetAsync<PagedResult<PostDto>>(
                $"api/Posts/ListByTag/{tagId}?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<PagedResult<PostDto>>> ListByCategory(int categoryId, ListPaginatedRequest request)
            => await GetAsync<PagedResult<PostDto>>(
                $"api/Posts/ListByCategory/{categoryId}?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<bool>> ToggleLikePost(TogglePostLikeRequest request)
            => await PostAsync<bool>($"api/Posts/ToggleLikePost?PostId={request.PostId}");

        public async Task<Result<bool>> AddBookMark(AddBookMarkRequest request)
            => await PostAsync<bool>($"api/Posts/AddBookMark?PostId={request.PostId}");

        public async Task<Result<PagedResult<PostDto>>> ListBookMark(ListPaginatedRequest request)
            => await GetAsync<PagedResult<PostDto>>(
                $"api/Posts/ListBookMark?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<bool>> AddFeatured(AddFeaturedRequest dto)
            => await PostAsync<AddFeaturedRequest, bool>("api/Posts/AddFeatured", dto);

        public async Task<Result<List<FeaturedPostDto>>> ListFeatured()
            => await GetAsync<List<FeaturedPostDto>>("api/Posts/ListFeatured");

        public async Task<Result<int>> AddComment(AddCommentRequest dto)
            => await PostAsync<AddCommentRequest, int>("api/Posts/AddComment", dto);

        public async Task<Result<int>> UpdateComment(UpdateCommentRequest dto)
            => await UpdateAsync<UpdateCommentRequest, int>("api/Posts/UpdateComment", dto);

        public async Task<Result<bool>> ToggleLikeComment(ToggleCommentLikeRequest dto)
            => await PostAsync<ToggleCommentLikeRequest, bool>("api/Posts/ToggleLikeComment", dto);

        public async Task<Result<List<RecentActivityItemDto>>> GetRecentActivity(int limit = 5, int daysBack = 7)
            => await GetAsync<List<RecentActivityItemDto>>($"api/Posts/GetRecentActivity?limit={limit}&daysBack={daysBack}");

        public async Task<Result<StatisticsDto>> GetPublicStatistics()
            => await GetAsync<StatisticsDto>("api/Posts/GetPublicStatistics");

        public async Task<Result<StatisticsDto>> GetStatistics()
            => await GetAsync<StatisticsDto>("api/Posts/GetStatistics");
    }
}