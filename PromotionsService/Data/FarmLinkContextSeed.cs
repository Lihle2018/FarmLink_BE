using FarmLink.Shared.Enumarations;
using MongoDB.Driver;
using PromotionsService.Enumarations;
using PromotionsService.Models;

namespace PromotionsService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Promotion> Collection)
        {
            bool existOrder = Collection.Find(p => true).Any();
            if (!existOrder)
                Collection.InsertManyAsync(GetPreconfiguredPromotions());
        }
        private static IEnumerable<Promotion> GetPreconfiguredPromotions()
        {
            return new List<Promotion>
    {
        new Promotion
        {
            Name = "Summer Sale",
            Description = "Get 10% off on all summer products",
            Code = "SUMMER10",
            DiscountAmount = 0.10m,
            StartDate = DateTime.Now.AddDays(1), // Starts tomorrow
            EndDate = DateTime.Now.AddDays(30),  // Ends in 30 days
            Type = PromotionType.SeasonalSale,
            State = State.Active,
            CreatedBy = "Admin",
            CreatedDate = DateTime.Now,
            ModifiedBy = "Admin",
            ModifiedDate = DateTime.Now,
            TargetAudience = PromotionTargetAudienceRequestModel.AllCustomers
        },
        new Promotion
        {
            Name = "Holiday Special",
            Description = "Get 20% off on holiday gifts",
            Code = "HOLIDAY20",
            DiscountAmount = 0.20m,
            StartDate = DateTime.Now.AddDays(5),   
            EndDate = DateTime.Now.AddDays(45),
            Type = PromotionType.SeasonalSale,
            State = State.Active,
            CreatedBy = "Admin",
            CreatedDate = DateTime.Now,
            ModifiedBy = "Admin",
            ModifiedDate = DateTime.Now,
            TargetAudience = PromotionTargetAudienceRequestModel.AllCustomers
        }
    };
        }

    }
}
