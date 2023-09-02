using VendorService.Models;
using VendorService.Models.RequestModels;

namespace VendorService.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(string vendorId);

        Task<IEnumerable<Vendor>> GetVendorsByLocationAsync(string locationId);

        Task<IEnumerable<Vendor>> GetVendorsByProductAsync(string productId);

        Task<Vendor> CreateVendorAsync(VendorRequestModel Request);

        Task<Vendor> UpdateVendorAsync(VendorRequestModel vendor);

        Task<long> DeleteVendorAsync(string vendorId);
    }

}
