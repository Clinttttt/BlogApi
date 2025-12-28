using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Posts.GetPostPaged;
using BlogApi.Application.Request.Posts;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface IPostClientService
    {
        Task<Result<int>> Create(PostRequest dto);
        Task<Result<PostWithCommentsDto>> Get(GetPostWithCommentsRequest PostId);
        Task<Result<int>> Update(UpdatePostRequest dto);
        Task<Result<bool>> Archived(int Id);
        Task<Result<bool>> Delete(int Id);
        Task<Result<List<PostDto>>> GetPostPage(GetPostPagedQuery request);
        Task<Result<bool>> PostLike(int Id);
        Task<Result<List<PostDto>>> GetPostByTag(int Id);
        Task<Result<List<PostDto>>> GetRecentPost();
        Task<Result<int>> CreateComment(CommentRequest dto);
        Task<Result<int>> UpdateComment(UpdateCommentRequest dto);
        Task<Result<bool>> CommentLike(ToggleCommentLikeRequest dto);
        Task<Result<bool>> AddBookMark(AddBookMarkRequest dto);
        Task<Result<List<PostDto>>> GetBookMark();
        Task<Result<PostDashboardDto>?> PostDashboard();
        Task<Result<bool>> AddFeatured(AddFeaturedRequest dto);
        Task<Result<FeaturedPostDto>> GetFeatured();
        }
}
