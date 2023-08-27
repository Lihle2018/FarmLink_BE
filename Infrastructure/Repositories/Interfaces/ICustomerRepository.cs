using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        bool Connect();
        Task<Customer> AddCustomerAsync(CustomerRequestModel Request);
        Task<Customer> UpdateCustomerAsync(CustomerRequestModel Request);
        Task<long> DeleteCustomerAsync(string Id);
        Task<long> SoftDeleteCustomerAsync(string Id);
        Task<Customer> GetCustomerByIdAsync(string Id);
        Task<List<Customer>> GetAllCustomers();

    }
}
