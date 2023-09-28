using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CommunicationService.Models
{
    public class ChatRoom
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> UserIds { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set;}
    }
}
