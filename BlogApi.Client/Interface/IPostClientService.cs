using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Posts.GetPostPaged;
using BlogApi.Application.Request.Posts;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface IPostClientService
    {
        Task<Result<int>> Create(PostRequest dto);
        Task<Result<PostWithCommentsDto>> Get(int PostId);
        Task<Result<int>> Update(UpdatePostRequest dto);
        Task<Result<bool>> Archived(int Id);
        Task<Result<bool>> Delete(int Id);
        Task<Result<List<PostDto>>> GetPostPage(GetPostPagedQuery request);
        Task<Result<bool>> PostLike(int Id);
        Task<Result<List<PostDto>>> GetPostByTag(int Id);
        Task<Result<List<PostDto>>> GetRecentPost(int Id);
        Task<Result<int>> CreateComment(CommentRequest dto);
        Task<Result<int>> UpdateComment(UpdateCommentRequest dto);

        }
}
