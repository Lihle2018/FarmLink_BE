using FarmLink.CustomerService.Models.RequestModels;
using FarmLink.CustomerService.Models.ResponseModels;

namespace CustomerService.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerResponseModel> AddCustomerAsync(CustomerRequestModel Request);
        Task<CustomerResponseModel> UpdateCustomerAsync(CustomerRequestModel Request);
        Task<long> DeleteCustomerAsync(string Id);
        Task<long> SoftDeleteCustomerAsync(string Id);
        Task<CustomerResponseModel> GetCustomerAsync(string Id);
        Task<IEnumerable<CustomerResponseModel>> GetCustomersAsync();

    }
}
