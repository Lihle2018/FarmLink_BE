using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryResponseModel>> GetCategoriesAsync();
        Task<CategoryResponseModel> GetCategoryAsync(string categoryId);
        Task<CategoryResponseModel> CreateCategoryAsync(CategoryRequestModel Request);
        Task<CategoryResponseModel> UpdateCategoryAsync(CategoryRequestModel Request);
        Task<long> DeleteCategoryAsync(string categoryId);
    }
}
