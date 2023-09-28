using MongoDB.Driver;
using ProductService.Data.Interfaces;
using ProductService.Models;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
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

        public async Task<CategoryResponseModel> CreateCategoryAsync(CategoryRequestModel Request)
        {
            try
            {
                var category = new Category(Request);
                await _context.Categories.InsertOneAsync(category);
                return new CategoryResponseModel(category);
            }
            catch (Exception e)
            {
                return new CategoryResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteCategoryAsync(string categoryId)
        {
            var result =await _context.Categories.DeleteOneAsync(categoryId);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<CategoryResponseModel>> GetCategoriesAsync()
        {
            try
            {

                var results = await _context.Categories.Find(c => true).ToListAsync();
                return results.Select(c=>new CategoryResponseModel(c));
            }
            catch (Exception e)
            {
                return new[] {new CategoryResponseModel(null,e.Message, true)};
            }
        }

        public async Task<CategoryResponseModel> GetCategoryAsync(string categoryId)
        {
            try
            {
                var results = await _context.Categories.FindAsync(c => c.Id == categoryId);
                return new CategoryResponseModel(results.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new CategoryResponseModel(null, e.Message, true);
            }
        }

        public async Task<CategoryResponseModel> UpdateCategoryAsync(CategoryRequestModel category)
        {
            try
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
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var updatedCategory = await _context.Categories.FindOneAndUpdateAsync(filter, update, options);

                return new CategoryResponseModel(updatedCategory);
            }
            catch (Exception e)
            {
                return new CategoryResponseModel(null, e.Message, true);
            }
        }
    }
}
