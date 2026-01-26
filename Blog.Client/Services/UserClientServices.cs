
using Blog.Application.Queries.User.Get;
using Blog.Application.Queries.User.GetListAuthor;
using BlogApi.Application.Commands.Newsletter.UnSubscribeToNewsletter;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Request.Posts;
using BlogApi.Application.Request.User;
using BlogApi.Client.Helper;
using BlogApi.Client.Interface;
using BlogApi.Domain.Common;
using Mapster;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.Net;
using System.Runtime.InteropServices;

namespace BlogApi.Client.Services
{
    public class UserClientServices(HttpClient httpClient) : HandleResponse(httpClient), IUserClientServices
    {
        public async Task<Result<UserProfileDto>> GetCurrentUser() => await GetAsync<UserProfileDto>("api/User/GetCurrentUser");
        public async Task<Result<bool>> UnSubscribeToNewsletter(string command) => await GetAsync<bool>($"api/User/unsubscribe/{command}");
        public async Task<Result<bool>> Add(UserInfoRequest dto) => await PostAsync<UserInfoRequest, bool>("api/User/Add", dto);
        public async Task<Result<bool>> Update(UserInfoRequest dto) =>  await UpdateAsync<UserInfoRequest, bool>("api/User/Update", dto);
        public async Task<Result<UserDashboardDto>> Get(Guid UserId) => await GetAsync<UserDashboardDto>($"api/User/Get/{UserId}");
        public async Task<Result<UserInfoDto>> GetUserInfo() => await GetAsync<UserInfoDto>("api/User/GetUserInfo");
        public async Task<Result<List<AuthorDto>>> GetTopAuthors() => await GetAsync<List<AuthorDto>>("api/User/GetTopAuthors");
        public async Task<Result<PagedResult<AuthorStatDto>>> GetListing(ListPaginatedRequest request) => await GetAsync<PagedResult<AuthorStatDto>>($"api/User/GetListing?PageNumber={request.PageNumber}&PageSize={request.PageSize}");
    }
}
