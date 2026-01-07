using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Common.Interfaces
{
    public interface IPostHubService
    {
        Task BroadcastViewCountUpdate(int postId, int viewCount);
        Task BroadcastNewPost(string postTitle);
        Task BroadcastSentComment(int PostId, string Content);
    }
}
