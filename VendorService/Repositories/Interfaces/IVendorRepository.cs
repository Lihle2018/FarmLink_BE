using VendorService.Models;
using VendorService.Models.RequestModels;
using VendorService.Models.ResponseModels;

namespace VendorService.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<VendorResponseModel>> GetVendorsAsync();

        Task<VendorResponseModel> GetVendorByIdAsync(string vendorId);

        Task<IEnumerable<VendorResponseModel>> GetVendorsByLocationAsync(string locationId);

        Task<IEnumerable<VendorResponseModel>> GetVendorsByProductAsync(string productId);

        Task<VendorResponseModel> CreateVendorAsync(VendorRequestModel Request);

        Task<VendorResponseModel> UpdateVendorAsync(VendorRequestModel vendor);

        Task<long> DeleteVendorAsync(string vendorId);
    }

}
