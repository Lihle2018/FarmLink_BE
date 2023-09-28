using MongoDB.Driver;
using ReviewService.Data.Interfaces;
using ReviewService.Models;
using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;
using ReviewService.Repositories.Interfaces;

namespace ReviewService.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly IFarmLinkContext _context;
        public CommentsRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<CommentResponseModel> AddCommentAsync(CommentRequestModel Request)
        {
            try
            {
                var comment = new Comment(Request);
                await _context.Comments.InsertOneAsync(comment);
                return new CommentResponseModel(comment);
            }
            catch (Exception e)
            {
                return new CommentResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteCommentAsync(string commentId)
        {
            var result = await _context.Comments.DeleteOneAsync(commentId);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<CommentResponseModel>> GetCommentsAsync()
        {
            try
            {
                var results = await _context.Comments.Find(c => true).ToListAsync();
                return results.Select(c => new CommentResponseModel(c));
            }
            catch (Exception e)
            {
                return new[] { new CommentResponseModel(null, e.Message, true) };
            }
        }

        public async Task<CommentResponseModel> GetCommentByIdAsync(string commentId)
        {
            try
            {
                var result = await _context.Comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
                return new CommentResponseModel(result);
            }
            catch (Exception e)
            {
                return new CommentResponseModel(null, e.Message, true);
            }
        }

        public async Task<IEnumerable<CommentResponseModel>> GetCommentsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _context.Comments.Find(c => c.CreatingUserId == userId).ToListAsync();
                return result.Select(C => new CommentResponseModel(C));
            }
            catch (Exception e)
            {
                return new[] { new CommentResponseModel(null, e.Message, true) };
            }
        }

        public async Task<IEnumerable<CommentResponseModel>> GetCommentsForPostAsync(string postId)
        {
            try
            {
                var result = await _context.Comments.Find(c => c.ReferenceId == postId).ToListAsync();
                return result.Select(c => new CommentResponseModel(c));
            }
            catch (Exception e)
            {
                return new[] { new CommentResponseModel(null, e.Message, true) };
            }
        }

        public async Task<CommentResponseModel> UpdateCommentAsync(CommentRequestModel comment)
        {
            try
            {
                var filter = Builders<Comment>.Filter.Eq(c => c.Id, comment.Id);
                var update = Builders<Comment>.Update
                    .Set(c => c.Message, comment.Message)
                    .Set(c => c.CreatingUserId, comment.CreatingUserId)
                    .Set(c => c.CreatedDate, comment.CreatedDate)
                    .Set(c => c.ModifiedDate, DateTime.UtcNow.ToString())
                    .Set(c => c.ModifyingUser, comment.ModifyingUser)
                    .Set(c => c.ReferenceId, comment.ReferenceId)
                    .Set(c => c.CommentType, comment.CommentType)
                    .Set(c => c.State, comment.State);

                var options = new FindOneAndUpdateOptions<Comment>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var result = await _context.Comments.FindOneAndUpdateAsync(filter, update, options);
                return new CommentResponseModel(result);
            }
            catch (Exception e)
            {
                return new CommentResponseModel(null, e.Message, true);
            }
        }
    }
}
