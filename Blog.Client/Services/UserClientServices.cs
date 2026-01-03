using Azure;
using BlogApi.Application.Commands.Newsletter.UnSubscribeToNewsletter;
using BlogApi.Application.Dtos;
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
        public async Task<Result<bool>> AddUserInfo(UserInfoRequest dto) => await PostAsync<UserInfoRequest, bool>("api/User/AddUserInfo", dto);
        public async Task<Result<bool>> UpdateUserInfo(UserInfoRequest dto) =>  await UpdateAsync<UserInfoRequest, bool>("api/User/UpdateUserInfo", dto);
        public async Task<Result<UserInfoDto>> GetUserInfo() => await GetAsync<UserInfoDto>("api/User/GetUserInfo");  
    }
}
