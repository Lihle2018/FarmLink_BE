using FarmLink.Shared.Enumarations;
using FarmLink.Shared.RequestModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace FarmLink.Shared.Entiities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string Otp { get; set; }
        public string Salt { get; set; }
        public Address Address { get; set; }
        public string UpdatedAt { get; set; }
        public State UserState { get; set; }
        public string DeletedAt { get; set; }
        public User() { }
        public User(UserRequestModel user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            PhoneNumber = user.PhoneNumber;
            Role = user.Role;
            ProfilePicture = user.ProfilePicture;
            IsEmailVerified = user.IsEmailVerified;
            IsPhoneVerified = user.IsPhoneVerified;
            Otp = user.Otp;
            Salt = user.Salt;
            Address = user.Address;
            UpdatedAt = user.UpdatedAt;
            UserState = user.UserState;
            DeletedAt = user.DeletedAt;
        }

    }
    public enum Role
    {
        Admin=0, Customer=1,Vendor=2,FarmersMarket=3
    }
}
