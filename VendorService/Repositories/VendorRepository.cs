using MongoDB.Driver;
using VendorService.Data.Interfaces;
using VendorService.Models;
using VendorService.Models.RequestModels;
using VendorService.Models.ResponseModels;
using VendorService.Repositories.Interfaces;

namespace VendorService.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly IFarmLinkContext _context;
        public VendorRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<VendorResponseModel> CreateVendorAsync(VendorRequestModel Request)
        {
            try
            {
                var vendor = new Vendor(Request);
                await _context.Vendors.InsertOneAsync(vendor);
                return new VendorResponseModel(vendor);
            }
            catch (Exception e)
            {
                return new VendorResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteVendorAsync(string vendorId)
        {
            var result = await _context.Vendors.DeleteOneAsync(vendorId);
            return result.DeletedCount;
        }

        public async Task<VendorResponseModel> GetVendorByIdAsync(string vendorId)
        {
            try
            {
                var result = await _context.Vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
                return new VendorResponseModel(result);
            }
            catch (Exception e)
            {
                return new VendorResponseModel(null, e.Message, true);
            }
        }

        public async Task<IEnumerable<VendorResponseModel>> GetVendorsAsync()
        {
            try
            {
                var result = await _context.Vendors.Find(v => true).ToListAsync();
                return result.Select(v=>new VendorResponseModel(v));
            }
            catch (Exception e)
            {
                return new[] {new VendorResponseModel(null,e.Message, true)};
            }
        }

        public async Task<IEnumerable<VendorResponseModel>> GetVendorsByLocationAsync(string locationId)
        {
            try
            {
                var result = await _context.Vendors.Find(v => v.LocationId == locationId).ToListAsync();
                return result.Select(v=>new VendorResponseModel(v));
            }
            catch (Exception e)
            {
                return new[] { new VendorResponseModel(null, e.Message, true) };
            }
        }

        public async Task<IEnumerable<VendorResponseModel>> GetVendorsByProductAsync(string productId)
        {
            try
            {
                var result = await _context.Vendors.Find(v => v.ProductIds.Contains(productId)).ToListAsync();
                return result.Select(v=>new VendorResponseModel(v));
            }
            catch (Exception e)
            {
                return new[] { new VendorResponseModel(null, e.Message, true) };
            }
        }

        public async Task<VendorResponseModel> UpdateVendorAsync(VendorRequestModel vendor)
        {
            try
            {
                var filter = Builders<Vendor>.Filter.Eq(v => v.Id, vendor.Id);

                var update = Builders<Vendor>.Update
                    .Set(v => v.UserId, vendor.UserId)
                    .Set(v => v.Description, vendor.Description)
                    .Set(v => v.CreatedDate, vendor.CreatedDate)
                    .Set(v => v.ModifyingUser, vendor.ModifyingUser)
                    .Set(v => v.DateModified, DateTime.UtcNow)
                    .Set(v => v.LocationId, vendor.LocationId)
                    .Set(v => v.Location, vendor.Location)
                    .Set(v => v.ProductIds, vendor.ProductIds)
                    .Set(v => v.VendorTagIds, vendor.VendorTagIds)
                    .Set(v => v.MinimumOrderAmount, vendor.MinimumOrderAmount)
                    .Set(v => v.VendorName, vendor.VendorName)
                    .Set(v => v.ContactEmail, vendor.ContactEmail)
                    .Set(v => v.ContactPhone, vendor.ContactPhone)
                    .Set(v => v.LogoUrl, vendor.LogoUrl)
                    .Set(v => v.OperatingHours, vendor.OperatingHours)
                    .Set(v => v.AcceptedPaymentMethods, vendor.AcceptedPaymentMethods)
                    .Set(v => v.ReviewIds, vendor.ReviewIds)
                    .Set(v => v.OrderIds, vendor.OrderIds);

                var options = new FindOneAndUpdateOptions<Vendor>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert=false
                };
                var result= await _context.Vendors.FindOneAndUpdateAsync(filter, update, options);
                return new VendorResponseModel(result);
            }
            catch (Exception e)
            {
                return new VendorResponseModel(null, e.Message, true);
            }
        }
    }
}
