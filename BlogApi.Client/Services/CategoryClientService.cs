using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Category;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Microsoft.AspNetCore.Server.HttpSys;

namespace BlogApi.Client.Services
{
    public class CategoryClientService : ICategoryClientService
    {
        private readonly HttpClient _httpClient;
        public CategoryClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Result<bool>> Create(AddCategoryRequest CategoryName)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Category/CreateCategory", new { CategoryName });
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
        public async Task<Result<List<CategoryDto>>> GetListing()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/Category/GetCategory");
            if (result is null)
                return Result<List<CategoryDto>>.NotFound();
            return Result<List<CategoryDto>>.Success(result);

        }
        public async Task<Result<bool>> Delete(int Id)
        {
            var result = await _httpClient.DeleteFromJsonAsync<bool>($"api/Category/DeleteCategory/{Id}");
            return Result<bool>.Success(result);

        }
        public async Task<Result<bool>> Unlink(UnlinkCategoryRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Category/UnlinkCategory", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
        public async Task<Result<bool>> Link(LinkCategoryRequest dto)
        {
            var request = await _httpClient.PostAsJsonAsync("api/Category/linkCategory", dto);
            request.EnsureSuccessStatusCode();
            var result = await request.Content.ReadFromJsonAsync<bool>();
            return Result<bool>.Success(result);
        }
    }
}
