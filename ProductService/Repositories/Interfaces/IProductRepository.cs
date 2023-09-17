using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductResponseModel>> GetProductsAsync();
        Task<ProductResponseModel> GetProductByIdAsync(string productId);
        Task<IEnumerable<ProductResponseModel>> GetProductsByVendorAsync(string vendorId);
        Task<IEnumerable<ProductResponseModel>> GetProductsByCategoryAsync(string categoryId);
        Task<IEnumerable<ProductResponseModel>> SearchProductsAsync(string searchTerm);
        Task<ProductResponseModel> CreateProductAsync(ProductRequestModel product);
        Task<ProductResponseModel> UpdateProductAsync(ProductRequestModel product);
        Task<long> DeleteProductAsync(string productId);
    }
}
