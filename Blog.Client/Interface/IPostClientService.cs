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


        Task<Result<PagedResult<PostDto>>> ListForAdmin(ListForAdminPostsRequest request);
        Task<Result<PagedResult<PostDto>>> ListPublished(ListPublishedPostsRequest request);
        Task<Result<List<PostDto>>> ListByTag(int id);
        Task<Result<List<PostDto>>> ListByCategory(int id);

      
        Task<Result<bool>> ToggleLikePost(TogglePostLikeRequest request);
        Task<Result<bool>> AddBookMark(AddBookMarkRequest request);
        Task<Result<List<PostDto>>> ListBookMark();
        Task<Result<bool>> AddFeatured(AddFeaturedRequest dto);
        Task<Result<List<FeaturedPostDto>>> ListFeatured();

     
        Task<Result<int>> AddComment(AddCommentRequest dto);
        Task<Result<int>> UpdateComment(UpdateCommentRequest dto);
        Task<Result<bool>> ToggleLikeComment(ToggleCommentLikeRequest dto);



        Task<Result<StatisticsDto>> GetPublicStatistics();
        Task<Result<StatisticsDto>> GetStatistics();
    }
}