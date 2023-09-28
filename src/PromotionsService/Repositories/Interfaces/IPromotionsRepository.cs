using PromotionsService.Enumarations;
using PromotionsService.Models.RequestModels;
using PromotionsService.Models.ResponseModels;

namespace PromotionsService.Repositories.Interfaces
{

    public interface IPromotionsRepository
    {
        Task<PromotionResponseModel> CreatePromotionAsync(PromotionRequestModel promotion);

        Task<PromotionResponseModel> GetPromotionByIdAsync(string promotionId);

        Task<IEnumerable<PromotionResponseModel>> GetActivePromotionsAsync();

        Task<IEnumerable<PromotionResponseModel>> GetPromotionsByTypeAsync(PromotionType type);

        Task<IEnumerable<PromotionResponseModel>> GetPromotionsByTargetAudienceAsync(PromotionTargetAudience targetAudience);

        Task<PromotionResponseModel> UpdatePromotionAsync(PromotionRequestModel promotion);

        Task<long> DeletePromotionAsync(string promotionId);

        Task<IEnumerable<PromotionResponseModel>> GetPromotionsAsync();
    }
}
