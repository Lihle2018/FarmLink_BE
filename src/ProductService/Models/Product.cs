using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductService.Models.RequestModels;
using System.Numerics;

namespace ProductService.Models
{
    // <summary>
    /// Represents a product that can be sold at a farmer's market, such as fruits, vegetables, eggs, or meat.
    /// </summary>
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatingUser { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyingUser { get; set; }
        public string ModifiedDate { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string VendorId { get; set; }
        public string CategoryId { get; set; }
        public Product() { }
        public Product(ProductRequestModel model)
        {
            Id = model.Id;
        }
    }
}
