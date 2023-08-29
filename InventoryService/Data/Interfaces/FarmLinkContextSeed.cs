using InventoryService.Models;
using MongoDB.Driver;

namespace InventoryService.Data.Interfaces
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<InventoryItem> Collection)
        {
            bool existInventory = Collection.Find(p => true).Any();
            if (!existInventory)
                Collection.InsertManyAsync(GetPreconfiguredInventory());
        }
        private static IEnumerable<InventoryItem> GetPreconfiguredInventory()
        {
            return new List<InventoryItem>
    {
        new InventoryItem
        {
            Quantity = 10,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            ModifyingUser = "admin",
            CreatingUser = "admin"
        },
        new InventoryItem
        {
            Quantity = 15,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            ModifyingUser = "admin",
            CreatingUser = "admin"
        },
        new InventoryItem
        {
            Quantity = 8,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            ModifyingUser = "admin",
            CreatingUser = "admin"
        }
    };
        }

    }
}
