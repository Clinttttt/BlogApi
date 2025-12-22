using BlogApi.Application.Commands.Posts.CreatePost;
using static BlogApi.Domain.Enum.EntityEnum;

public class PostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public byte[]? Photo { get; set; }
    public string? PhotoContent { get; set; }
    public string? Author { get; set; }
    public Status Status { get; set; }
    public ReadingDuration ReadingDuration { get; set; }

    public IReadOnlyList<int> TagIds { get; set; } = Array.Empty<int>();

    public CreatePostCommand SetAddCommand(Guid userId)
        => new(
            Title,
            Content,
            CategoryId,
            userId,
            Photo,
            PhotoContent,
            Author,
            Status,
            ReadingDuration,
            TagIds
        );
}
