using FarmLink.Shared.ResponseModel;

namespace ReviewService.Models.ResponseModels
{
    public class RatingResponseModel:ResponseBase<Rating>
    {
        public RatingResponseModel(Rating rating, string message = null,bool error=false)
        {
            Data = rating;
            Message = message;
            Error = error;
        }
    }
}
