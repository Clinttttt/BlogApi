using BlogApi.Application.Commands.Posts.CreatePost;
using BlogApi.Application.Dtos;

using BlogApi.Application.Queries.Posts.GetPostPaged;
using BlogApi.Application.Queries.Posts.GetPostWithComments;
using BlogApi.Application.Request.Posts;
using BlogApi.Client.Dtos;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;

namespace BlogApi.Client.Services
{
    public class PostClientService : IPostClientService
    {
        private readonly HttpClient _httpClient;
        public PostClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<int>> Create(PostRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Posts/CreatePosts", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<int>();         
            return Result<int>.Success(result);
        }
        public async Task<Result<PostWithCommentsDto>> Get(int PostId)
        {
            var command = new GetPostWithCommentsQuery(PostId);
            var request = await _httpClient.GetFromJsonAsync<PostWithCommentsDto>($"api/Posts/GetPost?PostId={command.PostId}");
            if (request is null)
                return Result<PostWithCommentsDto>.NotFound();
            return Result<PostWithCommentsDto>.Success(request);
        }
        public async Task<Result<int>> Update(UpdatePostRequest dto)
        {
            var request = await _httpClient.PatchAsJsonAsync("api/Posts/UpdatePost", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<int>();
            return Result<int>.Success(result);
        }
        public async Task<Result<bool>> Archived(int Id)
        {
            var request = await _httpClient.PatchAsync($"api/Posts/ArchivedPost?Id={Id}", null);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);

        }
        public async Task<Result<bool>> Delete(int Id)
        {
            var request = await _httpClient.DeleteAsync($"api/Posts/DeletePost?PostId={Id}");
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
        public async Task<Result<List<PostDto>>> GetPostPage(GetPostPagedQuery request)
        {

            var result = await _httpClient.GetFromJsonAsync<List<PostDto>>($"api/Posts/GetAllPost?=PageNumber{request.PageNumber}&PageSize={request.PageSize}");
            if (result is null)
                return Result<List<PostDto>>.NotFound();
            return Result<List<PostDto>>.Success(result);
        }
        public async Task<Result<bool>> PostLike(int Id)
        {
            var request = await _httpClient.PostAsync($"api/Posts/PostLike?PostId={Id}", null);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
     
        public async Task<Result<List<PostDto>>> GetPostByTag(int Id)
        {
            var request = await _httpClient.PostAsync($"api/Posts/GetPostByTag?TagId={Id}",null);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<List<PostDto>>();
            if (result is null)
                return Result<List<PostDto>>.NotFound();
            return Result<List<PostDto>>.Success(result);
        }
        public async Task<Result<List<PostDto>>> GetRecentPost(int Id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<PostDto>>($"api/Posts/GetRecentPost?RecentPost={Id}");
            if (result is null)
                return Result<List<PostDto>>.NotFound();
            return Result<List<PostDto>>.Success(result);
        }
        public async Task<Result<int>> CreateComment(CommentRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Posts/CreateComment", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<int>();
            return Result<int>.Success(result);
        }
        public async Task<Result<int>> UpdateComment(UpdateCommentRequest dto)
        {
            var request = await _httpClient.PatchAsJsonAsync("api/Posts/UpdateComment", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<int>();
            return Result<int>.Success(result);
        }

    }
}
