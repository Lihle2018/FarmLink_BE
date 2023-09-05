using FarmLink.Shared.Entiities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VendorService.Models.RequestModels;
using VendorService.Enumarations;

namespace VendorService.Models
{
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyingUser { get; set; }
        public DateTime DateModified { get; set; }
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public List<string> ProductIds { get; set; }
        public List<string> VendorTagIds { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public string VendorName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string LogoUrl { get; set; }
        public List<OperatingHour> OperatingHours { get; set; }
        public List<PaymentMethod> AcceptedPaymentMethods { get; set; }
        public List<string> ReviewIds { get; set; }
        public List<string> OrderIds { get; set; }
        public Vendor() { }
        public Vendor(VendorRequestModel request)
        {
            Id = request.Id;
            UserId = request.UserId;
            Description = request.Description;
            CreatedDate= request.CreatedDate;
            ModifyingUser = request.ModifyingUser;
            DateModified = request.DateModified;
            LocationId = request.LocationId;
            Location = request.Location;
            ProductIds = request.ProductIds;
            VendorTagIds = request.VendorTagIds;
            MinimumOrderAmount = request.MinimumOrderAmount;
            VendorName = request.VendorName;    
            ContactEmail = request.ContactEmail;
            ContactPhone = request.ContactPhone;
            LogoUrl = request.LogoUrl;
            OperatingHours= request.OperatingHours;
            AcceptedPaymentMethods= request.AcceptedPaymentMethods;
            ReviewIds= request.ReviewIds;
            OrderIds = request.OrderIds;
        }
    }
}
