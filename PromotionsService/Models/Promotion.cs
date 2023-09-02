using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FarmLink.Shared.Enumarations;
using PromotionsService.Enumarations;
using PromotionsService.Models.RequestModels;

namespace PromotionsService.Models
{
    public class Promotion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Code { get; set; }

        public decimal DiscountAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public PromotionType Type { get; set; }

        public State State { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public PromotionTargetAudienceRequestModel TargetAudience { get; set; }

        public Promotion() { }
        public Promotion(PromotionRequestModel promotion)
        {
            Id = promotion.Id;
            Name = promotion.Name;
            Description = promotion.Description;
            Code = promotion.Code;
            DiscountAmount = promotion.DiscountAmount;
            StartDate = promotion.StartDate;
            EndDate = promotion.EndDate;
            Type = promotion.Type;
            State = promotion.State;
            CreatedBy = promotion.CreatedBy;
            CreatedDate = DateTime.UtcNow;
            ModifiedBy = promotion.ModifiedBy;
            ModifiedDate = DateTime.UtcNow;
            TargetAudience = promotion.TargetAudience;
        }
    }
}

