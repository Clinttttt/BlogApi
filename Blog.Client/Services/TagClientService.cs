

using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Tag;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace BlogApi.Client.Services
{
    public class TagClientService(HttpClient httpClient) : HandleResponse(httpClient), ITagClientService
    {
        public async Task<Result<int>> Create(AddTagRequest dto) => await PostAsync<AddTagRequest, int>("api/Tag/AddTag", dto);
        public async Task<Result<bool>> Delete(int TagId) => await DeleteAsync<bool>($"api/Tag/AddTag/{TagId}");
        public async Task<Result<List<TagDto>>> GetListing() => await GetAsync<List<TagDto>>("api/Tag/GetListing");
        public async Task<Result<List<TagDto>>> GetListingPostTag() => await GetAsync<List<TagDto>>("api/Tag/GetListingPostTag");
        public async Task<Result<bool>> AddTagsToPost(AddTagsToPostRequest dto) => await PostAsync<AddTagsToPostRequest, bool>("api/Tag/AddTagTopost", dto);
    }
}
