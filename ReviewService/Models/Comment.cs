using FarmLink.Shared.Enumarations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReviewService.Models.RequestModels;

namespace ReviewService.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatingUser { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifyingUser { get; set; }
        public string ReferenceId { get; set; }
        public Type CommentType { get; set; }
        public State State { get; set; }
        public Comment() { }
        public Comment(CommentRequestModel request)
        {
            Id = request.Id;
            Message = request.Message;
            CreatingUser = request.CreatingUser;
            CreatedDate = request.CreatedDate;
            ModifiedDate = request.ModifiedDate;
            ModifyingUser = request.ModifyingUser;
            ReferenceId = request.ReferenceId;
            CommentType = request.CommentType;
            State = request.State;
        }

    }
    public enum Type
    {
        Vendor = 0, Product = 1
    }
}
