using PromotionsService.Enumarations;
using PromotionsService.Models.RequestModels;
using PromotionsService.Models;

namespace PromotionsService.Repositories.Interfaces
{

    public interface IPromotionsRepository
    {
        Task<Promotion> CreatePromotionAsync(PromotionRequestModel promotion);

        Task<Promotion> GetPromotionByIdAsync(string promotionId);

        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();

        Task<IEnumerable<Promotion>> GetPromotionsByTypeAsync(PromotionType type);

        Task<IEnumerable<Promotion>> GetPromotionsByTargetAudienceAsync(PromotionTargetAudience targetAudience);

        Task<Promotion> UpdatePromotionAsync(PromotionRequestModel promotion);

        Task<long> DeletePromotionAsync(string promotionId);

        Task<IEnumerable<Promotion>> GetPromotionsAsync();
    }
}
