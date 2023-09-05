using MongoDB.Driver;
using ProductService.Data.Interfaces;
using ProductService.Models;
using ProductService.Models.RequestModels;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IFarmLinkContext _context;
        public CategoryRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Category> CreateCategoryAsync(CategoryRequestModel Request)
        {
            var category = new Category(Request);
            await _context.Categories.InsertOneAsync(category);
            return category;
        }

        public async Task<long> DeleteCategoryAsync(string categoryId)
        {
            var result =await _context.Categories.DeleteOneAsync(categoryId);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var results =await _context.Categories.FindAsync(c => true);
            return results.ToEnumerable();
        }

        public async Task<Category> GetCategoryAsync(string categoryId)
        {
            var results = await _context.Categories.FindAsync(c =>c.Id==categoryId);
            return results.FirstOrDefault();
        }

        public async Task<Category> UpdateCategoryAsync(CategoryRequestModel category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
            var update = Builders<Category>.Update
                .Set(c => c.Name, category.Name)
                .Set(c => c.Description, category.Description)
                .Set(c => c.ModifyingUser, category.ModifyingUser)
                .Set(c => c.ModifiedDate, DateTime.UtcNow)
                .Set(c => c.CreatedDate, category.CreatedDate)
                .Set(c => c.CreatingUser, category.CreatingUser);

            var options = new FindOneAndUpdateOptions<Category>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedCategory = await _context.Categories.FindOneAndUpdateAsync(filter, update, options);

            return updatedCategory;
        }
    }
}
