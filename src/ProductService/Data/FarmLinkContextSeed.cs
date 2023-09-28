using MongoDB.Driver;
using ProductService.Models;

namespace ProductService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Product> Products, IMongoCollection<Category> Categories)
        {
            bool existProduct = Products.Find(p => true).Any();
            if (!existProduct)
                Products.InsertManyAsync(GetPreconfiguredProducts());
            bool existsCategory = Categories.Find(c => true).Any();
            if (!existsCategory)
                Categories.InsertManyAsync(GetPreconfiguredCategories());
        }
        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
    {
        new Product
        {
            Name = "Product 1",
            Description = "Description for Product 1",
            Price = 10.99m,
            ImageUrl = "image1.jpg",
            VendorId = "5f86b6b48a72e83e589aa237",
            CategoryId = "5f86b6b48a72e83e589aa238",
            CreatingUser = "admin",
            CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ModifyingUser = "admin",
            ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        },
        new Product
        {
            Name = "Product 2",
            Description = "Description for Product 2",
            Price = 15.99m,
            ImageUrl = "image2.jpg",
            VendorId = "5f86b6b48a72e83e589aa237",
            CategoryId = "5f86b6b48a72e83e589aa238",
            CreatingUser = "admin",
            CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ModifyingUser = "admin",
            ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        },
        new Product
        {
            Name = "Product 3",
            Description = "Description for Product 3",
            Price = 20.99m,
            ImageUrl = "image3.jpg",
            VendorId = "5f86b6b48a72e83e589aa238",
            CategoryId = "5f86b6b48a72e83e589aa239",
            CreatingUser = "admin",
            CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ModifyingUser = "admin",
            ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        }
    };
        }

        private static IEnumerable<Category> GetPreconfiguredCategories()
        {
            var currentDate = DateTime.UtcNow;

            return new List<Category>
    {
        new Category
        {
            Name = "Category 1",
            Description = "Description for Category 1",
            CreatingUser = "Admin",
            CreatedDate = currentDate,
            ModifyingUser = "Admin",
            ModifiedDate = currentDate
        },
        new Category
        {
            Name = "Category 2",
            Description = "Description for Category 2",
            CreatingUser = "Admin",
            CreatedDate = currentDate,
            ModifyingUser = "Admin",
            ModifiedDate = currentDate
        },
        new Category
        {
            Name = "Category 3",
            Description = "Description for Category 3",
            CreatingUser = "Admin",
            CreatedDate = currentDate,
            ModifyingUser = "Admin",
            ModifiedDate = currentDate
        }
    };
        }

    }
}
