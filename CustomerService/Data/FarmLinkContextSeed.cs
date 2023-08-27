using FarmLink.CustomerService.Models;
using FarmLink.Shared.Entiities;
using FarmLink.Shared.Enumarations;
using MongoDB.Driver;

namespace CustomerService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Customer> Collection)
        {
            bool existCustomer = Collection.Find(p => true).Any();
            if (!existCustomer)
                Collection.InsertManyAsync(GetPreconfiguredCustomers());
        }
        private static IEnumerable<Customer> GetPreconfiguredCustomers()
        {
            return new List<Customer>
    {
        new Customer
        {
            Id = "1",
            UserId = "user123",
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            LocationId = "location456",
            Address = new Address
            {
                StreetLine1 = "123 Main St",
                StreetLine2 = "Apt 4B",
                City = "New York",
                State = "NY",
                PostalCode = "10001",
                Country = "USA"
            },
            OrderIds = new List<string> { "order1", "order2" },
            ReviewIds = new List<string> { "review1", "review2" },
            State = State.Active,
            DeletedAt = ""
        },
        new Customer
        {
            Id = "2",
            UserId = "user789",
            Name = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "987-654-3210",
            LocationId = "location789",
            Address = new Address
            {
                StreetLine1 = "456 Elm St",
                StreetLine2 = "Suite 203",
                City = "Los Angeles",
                State = "CA",
                PostalCode = "90001",
                Country = "USA"
            },
            OrderIds = new List<string> { "order3" },
            ReviewIds = new List<string> { "review3" },
            State = State.Active,
            DeletedAt = ""
        }
    };
        }

    }
}
