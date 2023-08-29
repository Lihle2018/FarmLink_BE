using InventoryService.Data.Interfaces;
using InventoryService.Models;
using InventoryService.Models.RequestModels;
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

        public async Task<InventoryItem> AddInventoryItemAsync(InventoryItemRequestModel inventoryItem)
        {
            var item = new InventoryItem(inventoryItem);
            await _context.Inventory.InsertOneAsync(item);
            return item;
        }

        public async Task<long> DeleteInventoryItemAsync(string id)
        {
            var result= await _context.Inventory.DeleteOneAsync(id);
            return result.DeletedCount;
        }

        public async Task<InventoryItem> GetInventoryItemByIdAsync(string id)
        {
            var result = await _context.Inventory.FindAsync(i => i.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync()
        {
            var result = await _context.Inventory.Find(i => true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemsByProductIdAsync(string productId)
        {
          var result= await _context.Inventory.Find(i=>i.ProductId== productId).ToListAsync();
            return result;
        }

        public async Task<InventoryItem> UpdateInventoryItemAsync(InventoryItemRequestModel inventoryItem)
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
                ReturnDocument = ReturnDocument.After
            };
            return await _context.Inventory.FindOneAndUpdateAsync(filter, update, options);
        }

    }
}
