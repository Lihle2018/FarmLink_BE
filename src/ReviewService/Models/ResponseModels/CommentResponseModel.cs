using FarmLink.Shared.ResponseModel;

namespace ReviewService.Models.ResponseModels
{
    public class CommentResponseModel:ResponseBase<Comment>
    {
        public CommentResponseModel(Comment comment, string message = null,bool error=false)
        {
            Data = comment;
            Message = message;
            Error = error;
        }
    }
}
