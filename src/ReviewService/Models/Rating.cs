using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ReviewService.Models.RequestModels;
using FarmLink.Shared.Enumarations;

namespace ReviewService.Models
{
    public class Rating
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int rating { get; set; }
        public string CreatingUser { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifyingUser { get; set; }
        public string ReferenceId { get; set; }
        public Type Type { get; set; }
        public State State { get; set; }
        public Rating() { }
        public Rating(RatingRequestModel request)
        {
            Id = request.Id;
            rating = request.rating;
            CreatingUser = request.CreatingUser;
            CreatedDate = request.CreatedDate;
            ModifiedDate = request.ModifiedDate;
            ModifyingUser = request.ModifyingUser;
            ReferenceId = request.ReferenceId;
            Type = request.Type;
            State = request.State;
        }
    }
}
