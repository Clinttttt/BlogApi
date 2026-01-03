using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Client.Dtos
{
    public record CreatePostDto(
     string Title,
     string Content,
     int CategoryId,
     Guid UserId,
     byte[]? Photo,
     string? PhotoContent,
     string? Author,
     Status Status,
     ReadingDuration ReadingDuration,
     IReadOnlyList<int> TagIds
 );
}
