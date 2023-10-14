using DnsClient;
using FarmLink.Shared.Entiities;
using MongoDB.Driver;
using VendorService.Enumarations;
using VendorService.Models;

namespace VendorService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Vendor> Collection)
        {
            bool existVendor = Collection.Find(p => true).Any();
            if (!existVendor)
                Collection.InsertManyAsync(GetPreconfiguredVendors());
        }
        private static IEnumerable<Vendor> GetPreconfiguredVendors()
        {
            return new List<Vendor>
    {
        new Vendor
        {
            UserId = "5f86b6b48a72e83e589aa23a",
            Description = "Vendor Description 1",
            LocationId = "5f86b6b48a72e83e589aa23b",
            Location = new Location
            {
                Name = "Vendor Location 1",
                Address = new Address
                {
                    StreetLine1 = "123 Main St",
                    City = "City 1",
                    State = "State 1",
                    PostalCode = "12345",
                    Country = "Country 1"
                },
                GeoLocation = new Geolocation
                {
                    Latitude = 12.3456,
                    Longitude = -78.9012
                }
            },
            ProductIds = new List<string>
            {
                "5f86b6b48a72e83e589aa23c",
                "5f86b6b48a72e83e589aa23d"
            },
            VendorTagIds = new List<string>
            {
                "5f86b6b48a72e83e589aa23e",
                "5f86b6b48a72e83e589aa23f"
            },
            MinimumOrderAmount = 50.00m,
            VendorName = "Vendor Name 1",
            ContactEmail = "vendor1@example.com",
            ContactPhone = "+1234567890",
            LogoUrl = "logo1.jpg",
            CreatedDate=DateTime.UtcNow,
            ModifyingUser="",
            DateModified=DateTime.UtcNow,
            OperatingHours = new List<OperatingHour>
            {
                new OperatingHour
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpeningTime = new TimeSpan(8, 0, 0),
                    ClosingTime = new TimeSpan(18, 0, 0),
                    CreatedDate = DateTime.UtcNow
                },
                new OperatingHour
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    OpeningTime = new TimeSpan(8, 0, 0),
                    ClosingTime = new TimeSpan(18, 0, 0),
                    CreatedDate = DateTime.UtcNow
                },

            },
            AcceptedPaymentMethods = new List<PaymentMethod>
            {
                PaymentMethod.CreditCard,
                PaymentMethod.DebitCard,
                PaymentMethod.PayPal
            },
            ReviewIds = new List<string>
            {
                "5f86b6b48a72e83e589aa240",
                "5f86b6b48a72e83e589aa241"
            },
            OrderIds = new List<string>
            {
                "5f86b6b48a72e83e589aa242",
                "5f86b6b48a72e83e589aa243"
            }
        },
        new Vendor
        {
            UserId = "5f86b6b48a72e83e589aa244",
            Description = "Vendor Description 2",
            LocationId = "5f86b6b48a72e83e589aa245",
            Location = new Location
            {
                Name = "Vendor Location 2",
                Address = new Address
                {
                    StreetLine1 = "456 Elm St",
                    City = "City 2",
                    State = "State 2",
                    PostalCode = "54321",
                    Country = "Country 2"
                },
                GeoLocation = new Geolocation
                {
                    Latitude = 34.5678,
                    Longitude = -56.7890
                }
            },
            ProductIds = new List<string>
            {
                "5f86b6b48a72e83e589aa246",
                "5f86b6b48a72e83e589aa247"
            },
            VendorTagIds = new List<string>
            {
                "5f86b6b48a72e83e589aa248",
                "5f86b6b48a72e83e589aa249"
            },
            MinimumOrderAmount = 75.00m,
            VendorName = "Vendor Name 2",
            ContactEmail = "vendor2@example.com",
            ContactPhone = "+9876543210",
            LogoUrl = "logo2.jpg",
            CreatedDate=DateTime.UtcNow,
            ModifyingUser="",
            DateModified=DateTime.UtcNow,
            OperatingHours = new List<OperatingHour>
            {
                new OperatingHour
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    OpeningTime = new TimeSpan(9, 0, 0),
                    ClosingTime = new TimeSpan(19, 0, 0)
                },
                new OperatingHour
                {
                    DayOfWeek = DayOfWeek.Thursday,
                   OpeningTime = new TimeSpan(9, 0, 0),
                    ClosingTime = new TimeSpan(19, 0, 0)
                },
            },
            AcceptedPaymentMethods = new List<PaymentMethod>
            {
                PaymentMethod.CreditCard,
                PaymentMethod.PayPal,
                PaymentMethod.Cryptocurrency
            },
            ReviewIds = new List<string>
            {
                "5f86b6b48a72e83e589aa24a",
                "5f86b6b48a72e83e589aa24b"
            },
            OrderIds = new List<string>
            {
                "5f86b6b48a72e83e589aa24c",
                "5f86b6b48a72e83e589aa24d"
            }
        }
    };
        }

    }
}
