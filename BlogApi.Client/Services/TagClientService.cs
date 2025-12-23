using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Tag;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using System.Net.Http.Json;

namespace BlogApi.Client.Services
{
    public class TagClientService : ITagClientService
    {
        private readonly HttpClient _httpClient;
        public TagClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        } 
        public async Task<Result<int>> Create(AddTagRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Tag/AddTag", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<int>();
            return Result<int>.Success(result);
        }
        public async Task<Result<bool>> Delete(int TagId)
        {
            var result = await _httpClient.DeleteFromJsonAsync<bool>($"api/Tag/AddTag/{TagId}");     
            return Result<bool>.Success(result);
        }
        public async Task<Result<List<TagDto>>> GetAllTags()
        {
            var result = await _httpClient.GetFromJsonAsync<List<TagDto>>("api/Tag/GetAllTags");
            if (result is null)
                return Result<List<TagDto>>.NotFound();
            return Result<List<TagDto>>.Success(result);
        }
        public async Task<Result<bool>> AddTagsToPost(AddTagsToPostRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Tag/AddTagTopost", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
    }
}
