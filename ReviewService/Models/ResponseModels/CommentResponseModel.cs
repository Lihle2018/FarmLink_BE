using FarmLink.Shared.ResponseModel;

namespace ReviewService.Models.ResponseModels
{
    public class CommentResponseModel:ResponseBase<Comment>
    {
        public CommentResponseModel(Comment comment, string message = null)
        {
            Data = comment;
            Message = message;
        }
    }
}
