using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FarmLink.Shared.Entiities
{
    /// <summary>
    /// Represents a vendor who sells products at the farmer's market.
    /// </summary>
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public List<string> ProductIds { get; set; }
        public List<string> VendorTagIds { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public bool AcceptsCreditCard { get; set; }
    }
}
