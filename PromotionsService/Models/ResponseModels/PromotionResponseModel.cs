using FarmLink.Shared.ResponseModel;

namespace PromotionsService.Models.ResponseModels
{
    public class PromotionResponseModel:ResponseBase<Promotion>
    {
        public PromotionResponseModel(Promotion promotion,string message=null,bool error=false)
        {
            Data = promotion;
            Message = message;
            Error = error;
        }
    }
}
