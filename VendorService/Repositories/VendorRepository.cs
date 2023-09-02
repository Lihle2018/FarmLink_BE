using VendorService.Models;
using VendorService.Models.RequestModels;
using VendorService.Repositories.Interfaces;

namespace VendorService.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        public Task<Vendor> CreateVendorAsync(VendorRequestModel Request)
        {
            throw new NotImplementedException();
        }

        public Task<long> DeleteVendorAsync(string vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vendor>> GetVendorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vendor>> GetVendorsByLocationAsync(string locationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vendor>> GetVendorsByProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<Vendor> UpdateVendorAsync(VendorRequestModel vendor)
        {
            throw new NotImplementedException();
        }
    }
}
