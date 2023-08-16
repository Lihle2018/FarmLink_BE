using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FarmLink.Shared.RequestModels;

namespace FarmLink.Shared.Entiities
{
    /// <summary>
    /// Represents a customer who purchases products from vendors at the farmer's market.
    /// </summary>
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LocationId { get; set; }
        public Address Address { get; set; }
        public List<string> OrderIds { get; set; }
        public List<string> ReviewIds { get; set; }
        public State State { get; set; }
        public string DeletedAt { get;set; }
        public Customer() { }
        public Customer(CustomerRequestModel Request)
        {
            Id= Request.Id;
            UserId= Request.UserId;
            Name= Request.Name;
            LastName= Request.LastName;
            Email= Request.Email;
            PhoneNumber= Request.PhoneNumber;
            LocationId= Request.LocationId;
            Address= Request.Address;
            OrderIds = Request.OrderIds;
            ReviewIds = Request.ReviewIds;
            State = Request.State;
            DeletedAt= Request.DeletedAt;
        }
    }
}
