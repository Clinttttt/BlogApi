using BlogApi.Application.Dtos;
using BlogApi.Application.Request.Category;
using BlogApi.Domain.Common;

namespace BlogApi.Client.Interface
{
    public interface ICategoryClientService
    {
        Task<Result<bool>> Create(AddCategoryRequest CategoryName);
        Task<Result<List<CategoryDto>>> GetListing();
        Task<Result<List<CategoryDto>>> GetListPostCategory();
        Task<Result<bool>> Delete(int Id);
        Task<Result<bool>> Unlink(UnlinkCategoryRequest dto);
        Task<Result<bool>> Link(LinkCategoryRequest dto);
    }
}
