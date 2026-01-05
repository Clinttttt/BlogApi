

using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Category;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Mapster;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace BlogApi.Client.Services
{
    public class CategoryClientService(HttpClient httpClient) : HandleResponse(httpClient), ICategoryClientService
    {
        public async Task<Result<bool>> Create(AddCategoryRequest dto) => await PostAsync<AddCategoryRequest, bool>("api/Category/CreateCategory", dto);
        public async Task<Result<List<CategoryDto>>> GetListing() => await GetAsync<List<CategoryDto>>("api/Category/GetListing");
        public async Task<Result<List<CategoryDto>>> GetListPostCategory() => await GetAsync<List<CategoryDto>>("api/Category/GetListPostCategory");
        public async Task<Result<bool>> Delete(int Id) => await DeleteAsync<bool>($"api/Category/DeleteCategory/{Id}");
        public async Task<Result<bool>> Unlink(UnlinkCategoryRequest dto) => await PostAsync<UnlinkCategoryRequest, bool>("api/Category/UnlinkCategory", dto);
        public async Task<Result<bool>> Link(LinkCategoryRequest dto) => await PostAsync<LinkCategoryRequest, bool>("api/Category/linkCategory", dto);    
    }
}
