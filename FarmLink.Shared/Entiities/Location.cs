using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FarmLink.Shared.Entiities
{
    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public Geolocation GeoLocation { get; set; }
        public List<string> VendorIds { get; set; }
    }
}
