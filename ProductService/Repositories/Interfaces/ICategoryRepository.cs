using ProductService.Models;
using ProductService.Models.RequestModels;

namespace ProductService.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(string categoryId);
        Task<Category> CreateCategoryAsync(CategoryRequestModel Request);
        Task<Category> UpdateCategoryAsync(CategoryRequestModel Request);
        Task<long> DeleteCategoryAsync(string categoryId);
    }
}
