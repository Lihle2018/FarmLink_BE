using MongoDB.Bson;
using MongoDB.Driver;
using ProductService.Data.Interfaces;
using ProductService.Models;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IFarmLinkContext _context;
        public ProductRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ProductResponseModel> CreateProductAsync(ProductRequestModel Request)
        {
            try
            {
                var product = new Product(Request);
                await _context.Products.InsertOneAsync(product);
                return new ProductResponseModel(product);
            }
            catch (Exception e)
            {
                return new ProductResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteProductAsync(string productId)
        {
           var result =await _context.Products.DeleteOneAsync(productId);
            return result.DeletedCount;
        }

        public async Task<ProductResponseModel> GetProductByIdAsync(string productId)
        {
            try
            {
                var result = await _context.Products.FindAsync(p => p.Id == productId);
                return new ProductResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new ProductResponseModel(null,e.Message, true);
            }
        }

        public async Task<IEnumerable<ProductResponseModel>> GetProductsAsync()
        {
            try
            {
                var result = await _context.Products.Find(p => true).ToListAsync();
                return result.Select(p=>new ProductResponseModel(p));
            }
            catch (Exception e)
            {
                return new[] {new ProductResponseModel(null,e.Message,true)};
            }
        }

        public async Task<IEnumerable<ProductResponseModel>> GetProductsByCategoryAsync(string categoryId)
        {
            try
            {
                var result = await _context.Products.Find(p => p.CategoryId == categoryId).ToListAsync();
                return result.Select(p => new ProductResponseModel(p));
            }
            catch (Exception e)
            {
                return new[] {new ProductResponseModel(null,e.Message,true)};
            }
        }

        public async Task<IEnumerable<ProductResponseModel>> GetProductsByVendorAsync(string vendorId)
        {
            try
            {
                var result = await _context.Products.Find(p => p.VendorId == vendorId).ToListAsync();
                return result.Select(p => new ProductResponseModel(p));
            }
            catch (Exception e)
            {
                return new[] {new ProductResponseModel(null,e.Message,true)};
            }
        }

        public async Task<IEnumerable<ProductResponseModel>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                var filter = Builders<Product>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));

                var products = await _context.Products
                    .Find(filter)
                    .ToListAsync();

                return products.Select(p => new ProductResponseModel(p)); ;
            }
            catch (Exception e)
            {
                return new[] {new ProductResponseModel(null,e.Message,true)};
            }
        }

        public async Task<ProductResponseModel> UpdateProductAsync(ProductRequestModel product)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
                var update = Builders<Product>.Update
                    .Set(p => p.Name, product.Name)
                    .Set(p => p.Description, product.Description)
                    .Set(p => p.ModifyingUser, product.ModifyingUser)
                    .Set(p => p.CreatingUser, product.CreatingUser)
                    .Set(p => p.ModifiedDate, DateTime.UtcNow.ToString())
                    .Set(p => p.Price, product.Price)
                    .Set(p => p.ImageUrl, product.ImageUrl)
                    .Set(p => p.VendorId, product.VendorId)
                    .Set(p => p.CategoryId, product.CategoryId);

                var options = new FindOneAndUpdateOptions<Product>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert=false
                };

                var result =await _context.Products.FindOneAndUpdateAsync(filter, update, options);
                return new ProductResponseModel(result);
            }
            catch (Exception e)
            {
                return new ProductResponseModel(null, e.Message, true);
            }
        }
    }
}
