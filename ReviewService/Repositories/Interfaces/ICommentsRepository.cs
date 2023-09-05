using ReviewService.Models;
using ReviewService.Models.RequestModels;

namespace ReviewService.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<Comment> GetCommentByIdAsync(string commentId);
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId);
        Task<IEnumerable<Comment>> GetCommentsForPostAsync(string postId);
        Task<Comment> AddCommentAsync(CommentRequestModel comment);
        Task<Comment> UpdateCommentAsync(CommentRequestModel comment);
        Task<long> DeleteCommentAsync(string commentId);
    }
}
