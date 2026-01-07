/*using Microsoft.AspNetCore.SignalR;
using Blog.Api.Hubs;
using Blog.Application.Common.Interfaces;

public class PostNotifier : IPostNotifier
{
    private readonly IHubContext<PostHub> _hub;

    public PostNotifier(IHubContext<PostHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyViewCountUpdated(int postId, int viewCount)
    {
        await _hub.Clients.Group($"post-{postId}")
            .SendAsync("ViewCountUpdated", viewCount);
    }
}
*/