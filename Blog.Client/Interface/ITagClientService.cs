using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Tag;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface ITagClientService
    {
        Task<Result<int>> Create(AddTagRequest dto);
        Task<Result<bool>> Delete(int TagId);
        Task<Result<List<TagDto>>> GetListing();
        Task<Result<bool>> AddTagsToPost(AddTagsToPostRequest dto);
        Task<Result<List<TagDto>>> GetListingPostTag();
    }
}
