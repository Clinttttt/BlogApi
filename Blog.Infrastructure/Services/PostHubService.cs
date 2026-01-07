using Blog.Application.Common.Interfaces;
using Blog.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class PostHubService : IPostHubService
    {
        private readonly IHubContext<PostHub> _hubContext;

        public PostHubService(IHubContext<PostHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastViewCountUpdate(int postId, int viewCount)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveViewCountUpdate", postId, viewCount);
        }
        public async Task BroadcastSentComment(int PostId, string Content)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveSentMessage", PostId, Content);
        }
        

        public async Task BroadcastNewPost(string postTitle)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNewPost", postTitle);
        }
    }
}
