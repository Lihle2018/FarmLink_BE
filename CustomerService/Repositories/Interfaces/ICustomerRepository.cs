using FarmLink.CustomerService.Models.RequestModels;
using FarmLink.CustomerService.Models;

namespace CustomerService.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(CustomerRequestModel Request);
        Task<Customer> UpdateCustomerAsync(CustomerRequestModel Request);
        Task<long> DeleteCustomerAsync(string Id);
        Task<long> SoftDeleteCustomerAsync(string Id);
        Task<Customer> GetCustomerAsync(string Id);
        Task<IEnumerable<Customer>> GetCustomersAsync();

    }
}
