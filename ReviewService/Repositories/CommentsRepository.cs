using MongoDB.Driver;
using ReviewService.Data.Interfaces;
using ReviewService.Models;
using ReviewService.Models.RequestModels;
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
        public async Task<Comment> AddCommentAsync(CommentRequestModel Request)
        {
            var comment = new Comment(Request);
            await _context.Comments.InsertOneAsync(comment);
            return comment;
        }

        public async Task<long> DeleteCommentAsync(string commentId)
        {
            var result = await _context.Comments.DeleteOneAsync(commentId);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            var results = await _context.Comments.Find(c => true).ToListAsync();
            return results;
        }

        public async Task<Comment> GetCommentByIdAsync(string commentId)
        {
            var result = await _context.Comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            var result = await _context.Comments.Find(c => c.CreatingUserId == userId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(string postId)
        {
            var result = await _context.Comments.Find(c => c.ReferenceId == postId).ToListAsync();
            return result;
        }

        public async Task<Comment> UpdateCommentAsync(CommentRequestModel comment)
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
                ReturnDocument = ReturnDocument.After
            };

            return await _context.Comments.FindOneAndUpdateAsync(filter, update, options);
        }

    }
}
