
using BlogApi.Application.Commands.User.AddUserInfo;
using BlogApi.Application.Commands.User.UpdateUserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.User
{
    public class UserInfoRequest
    {
        public string? Bio { get; set; }
        public string? FullName { get; set; }
        public byte[]? Photo { get; set; }
        public string? PhotoContentType { get; set; }
        public AddUserInfoCommand AddUserInfoCommand(Guid UserId)
            => new(Bio, FullName, Photo, PhotoContentType, UserId);
        public UpdateUserInfoCommand UpdateUserInfoCommand(Guid UserId)
           => new(Bio, FullName, Photo, PhotoContentType,UserId);
    }
}
