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



        public async Task<Result<PagedResult<PostDto>>> ListForAdmin(ListForAdminPostsRequest request)
           => await GetAsync<PagedResult<PostDto>>($"api/Posts/ListForAdmin?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<PagedResult<PostDto>>> ListPublished(ListPublishedPostsRequest request)
            => await GetAsync<PagedResult<PostDto>>($"api/Posts/ListPublished?PageNumber={request.PageNumber}&PageSize={request.PageSize}");

        public async Task<Result<List<PostDto>>> ListByTag(int id)
            => await GetAsync<List<PostDto>>($"api/Posts/ListByTag/{id}");

        public async Task<Result<List<PostDto>>> ListByCategory(int id)
            => await GetAsync<List<PostDto>>($"api/Posts/ListByCategory/{id}");

        
        public async Task<Result<bool>> ToggleLikePost(TogglePostLikeRequest request)
            => await PostAsync<bool>($"api/Posts/ToggleLikePost?PostId={request.PostId}");

        public async Task<Result<bool>> AddBookMark(AddBookMarkRequest request)
            => await PostAsync<bool>($"api/Posts/AddBookMark?PostId={request.PostId}");

        public async Task<Result<List<PostDto>>> ListBookMark()
            => await GetAsync<List<PostDto>>("api/Posts/ListBookMark");

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


        public async Task<Result<StatisticsDto>> GetPublicStatistics()
            => await GetAsync<StatisticsDto>("api/Posts/GetPublicStatistics");

        public async Task<Result<StatisticsDto>> GetStatistics()
            => await GetAsync<StatisticsDto>("api/Posts/GetStatistics");
    }
}