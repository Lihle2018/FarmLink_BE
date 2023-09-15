using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;

namespace ReviewService.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<CommentResponseModel>> GetCommentsAsync();
        Task<CommentResponseModel> GetCommentByIdAsync(string commentId);
        Task<IEnumerable<CommentResponseModel>> GetCommentsByUserIdAsync(string userId);
        Task<IEnumerable<CommentResponseModel>> GetCommentsForPostAsync(string postId);
        Task<CommentResponseModel> AddCommentAsync(CommentRequestModel comment);
        Task<CommentResponseModel> UpdateCommentAsync(CommentRequestModel comment);
        Task<long> DeleteCommentAsync(string commentId);
    }
}
