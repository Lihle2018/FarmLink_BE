using InventoryService.Data.Interfaces;
using InventoryService.Models;
using InventoryService.Models.RequestModels;
using InventoryService.Models.ResponseModels;
using InventoryService.Repositories.Interfaces;
using MongoDB.Driver;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IFarmLinkContext _context;

        public InventoryRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InventoryItemResponseModel> AddInventoryItemAsync(InventoryItemRequestModel inventoryItem)
        {
            try
            {
                var item = new InventoryItem(inventoryItem);
                await _context.Inventory.InsertOneAsync(item);
                return new InventoryItemResponseModel(item);
            }
            catch (Exception e)
            {
                return new InventoryItemResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteInventoryItemAsync(string id)
        {
            var result= await _context.Inventory.DeleteOneAsync(id);
            return result.DeletedCount;
        }

        public async Task<InventoryItemResponseModel> GetInventoryItemByIdAsync(string id)
        {
            try
            {
                var result = await _context.Inventory.FindAsync(i => i.Id == id);
                return new InventoryItemResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new InventoryItemResponseModel(null,e.Message,true);
            }
        }

        public async Task<IEnumerable<InventoryItemResponseModel>> GetInventoryItemsAsync()
        {
            try
            {
                var result = await _context.Inventory.Find(i => true).ToListAsync();
                return result.Select(i => new InventoryItemResponseModel(i));
            }
            catch (Exception e)
            {
                return new[] { new InventoryItemResponseModel(null, e.Message, true) };
            }
        }

        public async Task<IEnumerable<InventoryItemResponseModel>> GetInventoryItemsByProductIdAsync(string productId)
        {
            try
            {
                var result = await _context.Inventory.Find(i => i.ProductId == productId).ToListAsync();
                return result.Select(i => new InventoryItemResponseModel(i));
            }
            catch (Exception e)
            {
                return new[] { new InventoryItemResponseModel(null, e.Message, true) };
            }
        }

        public async Task<InventoryItemResponseModel> UpdateInventoryItemAsync(InventoryItemRequestModel inventoryItem)
        {
            try
            {
                var filter = Builders<InventoryItem>.Filter.Eq(i => i.Id, inventoryItem.Id);
                var update = Builders<InventoryItem>.Update
                    .Set(i => i.ProductId, inventoryItem.ProductId)
                    .Set(i => i.Quantity, inventoryItem.Quantity)
                    .Set(i => i.ModifyingUser, inventoryItem.ModifyingUser)
                    .Set(i => i.ModifiedDate, DateTime.UtcNow)
                    .Set(i => i.CreatingUser, inventoryItem.CreatingUser)
                    .Set(i => i.CreatedDate, inventoryItem.CreatedDate);

                var options = new FindOneAndUpdateOptions<InventoryItem>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert=false
                };
                var result= await _context.Inventory.FindOneAndUpdateAsync(filter, update, options);
                return new InventoryItemResponseModel(result);
            }
            catch (Exception e)
            {
                return new InventoryItemResponseModel(null, e.Message, true);
            }
        }

    }
}
