using BlogApi.Application.Dtos;
using BlogApi.Application.Request.User;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface IUserClientServices
    {
        Task<Result<UserProfileDto>> GetCurrentUser();
        Task<Result<bool>> UnSubscribeToNewsletter(string command);
        Task<Result<bool>> AddUserInfo(UserInfoRequest dto);
        Task<Result<bool>> UpdateUserInfo(UserInfoRequest dto);
        Task<Result<UserInfoDto>> GetUserInfo();
    }
}
