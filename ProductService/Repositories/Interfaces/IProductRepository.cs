using ProductService.Models;
using ProductService.Models.RequestModels;
using System.Buffers.Text;

namespace ProductService.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string productId);
        Task<IEnumerable<Product>> GetProductsByVendorAsync(string vendorId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<Product> CreateProductAsync(ProductRequestModel product);
        Task<Product> UpdateProductAsync(ProductRequestModel product);
        Task<long> DeleteProductAsync(string productId);
    }
}
