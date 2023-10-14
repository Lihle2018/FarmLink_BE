using InventoryService.Models.RequestModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryService.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifyingUser { get; set; }

        public string CreatingUser { get; set; }

        public InventoryItem() { }
        public InventoryItem(InventoryItemRequestModel request)
        {
            Id = request.Id;
            ProductId = request.ProductId;
            Quantity = request.Quantity;
            CreatedDate = request.CreatedDate;
            ModifiedDate = request.ModifiedDate;
            ModifyingUser = request.ModifyingUser;
            CreatingUser = request.CreatingUser;
        }
    }

}
