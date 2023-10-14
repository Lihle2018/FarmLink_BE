using MongoDB.Driver;
using VendorService.Models;

namespace VendorService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        public IMongoCollection<Vendor> Vendors { get; }
    }
}
