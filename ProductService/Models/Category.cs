using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ProductService.Models.RequestModels;

namespace ProductService.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatingUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyingUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Category() { }
        public Category(CategoryRequestModel request)
        {
            Id = request.Id;
            Name = request.Name;
            Description = request.Description;
            CreatingUser = request.CreatingUser;
            CreatedDate = DateTime.UtcNow;
            ModifyingUser = request.ModifyingUser;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
