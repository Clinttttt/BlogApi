using Application.Queries.GetRecentActivity;
using Blog.Application.Queries.GetRecentActivity;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;

using BlogApi.Application.Request.Posts;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface IPostClientService
    {

        Task<Result<int>> Create(CreatePostRequest dto);
        Task<Result<PostDetailDto>> Get(GetPostRequest request);
        Task<Result<int>> Update(UpdatePostRequest dto);
        Task<Result<bool>> Archive(ArchivePostRequest request);
        Task<Result<bool>> Delete(DeletePostRequest request);


        Task<Result<PagedResult<PostDto>>> ListPublishedForAdmin(ListPaginatedRequest request);
        Task<Result<PagedResult<PostDto>>> ListDraftForAdmin(ListPaginatedRequest request);
        Task<Result<PagedResult<PostDto>>> ListPublished(ListPaginatedRequest request);
        Task<Result<PagedResult<PostDto>>> ListByTag(int tagId, ListPaginatedRequest request);
        Task<Result<PagedResult<PostDto>>> ListByCategory(int Id, ListPaginatedRequest request);



        Task<Result<bool>> ToggleLikePost(TogglePostLikeRequest request);
        Task<Result<bool>> AddBookMark(AddBookMarkRequest request);
        Task<Result<PagedResult<PostDto>>> ListBookMark(ListPaginatedRequest request);
        Task<Result<bool>> AddFeatured(AddFeaturedRequest dto);
        Task<Result<List<FeaturedPostDto>>> ListFeatured();


        Task<Result<int>> AddComment(AddCommentRequest dto);
        Task<Result<int>> UpdateComment(UpdateCommentRequest dto);
        Task<Result<bool>> ToggleLikeComment(ToggleCommentLikeRequest dto);


        Task<Result<List<RecentActivityItemDto>>> GetRecentActivity(int limit = 4, int daysBack = 7);
        Task<Result<StatisticsDto>> GetPublicStatistics();
        Task<Result<StatisticsDto>> GetStatistics();
        Task<Result<bool>> TrackPostView(int? PostId);
    }
}