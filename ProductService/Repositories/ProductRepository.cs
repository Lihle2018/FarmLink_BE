using MongoDB.Bson;
using MongoDB.Driver;
using ProductService.Data.Interfaces;
using ProductService.Models;
using ProductService.Models.RequestModels;
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
        public async Task<Product> CreateProductAsync(ProductRequestModel Request)
        {
            var product =new Product(Request);
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<long> DeleteProductAsync(string productId)
        {
           var result =await _context.Products.DeleteOneAsync(productId);
            return result.DeletedCount;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            var result = await _context.Products.FindAsync(p => p.Id==productId);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var result = await _context.Products.Find(p => true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId)
        {
            var result = await _context.Products.FindAsync(p => p.CategoryId==categoryId);
            return result.ToEnumerable();
        }

        public async Task<IEnumerable<Product>> GetProductsByVendorAsync(string vendorId)
        {
            var result = await _context.Products.FindAsync(p => p.VendorId == vendorId);
            return result.ToEnumerable();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            var filter = Builders<Product>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));

            var products = await _context.Products
                .Find(filter)
                .ToListAsync();

            return products;
        }

        public async Task<Product> UpdateProductAsync(ProductRequestModel product)
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
                ReturnDocument = ReturnDocument.After
            };

            return await _context.Products.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}
