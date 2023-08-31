using FarmLink.Shared.Enumarations;
using MongoDB.Driver;
using ReviewService.Models;
using Type = ReviewService.Models.Type;

namespace ReviewService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Comment> commentsCollection, IMongoCollection<Rating> ratingCollection)
        {
            bool existComment = commentsCollection.Find(p => true).Any();
            if (!existComment)
                commentsCollection.InsertManyAsync(GetPreconfiguredComments());

            bool ratingExists = ratingCollection.Find(p => true).Any();
            if (!ratingExists)
                ratingCollection.InsertManyAsync(GetPreconfiguredRatings());
        }
        private static IEnumerable<Comment> GetPreconfiguredComments()
        {
            return new List<Comment>
        {
            new Comment
            {
                Message = "This is the first comment.",
                CreatingUserId = "user1",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user1",
                ReferenceId = "123",
                CommentType = Type.Vendor,
                State = State.Active
            },
            new Comment
            {
                Message = "Reply to the first comment.",
                CreatingUserId = "user2",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user2",
                ReferenceId = "123",
                CommentType = Type.Product,
                State = State.Active
            },
            new Comment
            {
                Message = "Another comment here.",
                CreatingUserId = "user3",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user3",
                ReferenceId = "456",
                CommentType = Type.Product,
                State = State.Active
            },
            new Comment
            {
                Message = "Inactive comment.",
                CreatingUserId = "user4",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user4",
                ReferenceId = "789",
                CommentType = Type.Product,
                State = State.Deleted
            }
            };
        }
        private static IEnumerable<Rating> GetPreconfiguredRatings()
        {
            return new List<Rating>
        {
            new Rating
            {
                rating = 5,
                CreatingUser = "user1",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user1",
                ReferenceId = "123",
                Type =Type.Product,
                State = State.Active
            },
            new Rating
            {
                rating = 3,
                CreatingUser = "user2",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user2",
                ReferenceId = "456",
                Type = Type.Vendor,
                State = State.Active
            },
            new Rating
            {
                rating = 4,
                CreatingUser = "user3",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user3",
                ReferenceId = "789",
                Type = Type.Product,
                State = State.Active
            },
            new Rating
            {
                rating = 2,
                CreatingUser = "user4",
                CreatedDate = DateTime.UtcNow.ToString(),
                ModifiedDate = DateTime.UtcNow.ToString(),
                ModifyingUser = "user4",
                ReferenceId = "123",
                Type = Type.Vendor,
                State = State.Active
            }
        };
        }
    }
}
