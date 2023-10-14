using FarmLink.IndentityService.Models;
using FarmLink.Shared.Entiities;
using FarmLink.Shared.Enumarations;
using MongoDB.Driver;

namespace IdentityService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<User> Collection)
        {
            bool existCustomer = Collection.Find(p => true).Any();
            if (!existCustomer)
                Collection.InsertManyAsync(GetPreconfiguredUsers());
        }
        private static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>
    {
        new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "hashed_password_1",
            PhoneNumber = "123-456-7890",
            Role = Role.Customer,
            ProfilePicture = "profile1.jpg",
            IsEmailVerified = true,
            IsPhoneVerified = true,
            Otp = "123456",
            Salt = "salt_1",
            Address = new Address
            {
                StreetLine1 = "123 Main St",
                StreetLine2 = "Apt 4B",
                City = "New York",
                State = "NY",
                PostalCode = "10001",
                Country = "USA"
            },
            UpdatedAt = DateTime.UtcNow.ToString(),
            UserState = State.Active,
            DeletedAt = ""
        },
        new User
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "hashed_password_2",
            PhoneNumber = "987-654-3210",
            Role = Role.Admin,
            ProfilePicture = "profile2.jpg",
            IsEmailVerified = true,
            IsPhoneVerified = true,
            Otp = "654321",
            Salt = "salt_2",
            Address = new Address
            {
                StreetLine1 = "456 Elm St",
                StreetLine2 = "Suite 203",
                City = "Los Angeles",
                State = "CA",
                PostalCode = "90001",
                Country = "USA"
            },
            UpdatedAt = DateTime.UtcNow.ToString(), 
            UserState = State.Active,
            DeletedAt = ""
        }
    };
        }

    }
}
