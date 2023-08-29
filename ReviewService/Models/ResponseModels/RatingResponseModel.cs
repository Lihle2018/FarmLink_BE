using FarmLink.Shared.ResponseModel;

namespace ReviewService.Models.ResponseModels
{
    public class RatingResponseModel:ResponseBase<Rating>
    {
        public RatingResponseModel(Rating rating, string message = null)
        {
            Data = rating;
            Message = message;
        }
    }
}
