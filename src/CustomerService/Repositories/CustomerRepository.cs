using CustomerService.Data.Interfaces;
using CustomerService.Repositories.Interfaces;
using FarmLink.CustomerService.Models;
using FarmLink.CustomerService.Models.RequestModels;
using FarmLink.CustomerService.Models.ResponseModels;
using FarmLink.Shared.Enumarations;
using MongoDB.Driver;

namespace CustomerService.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IFarmLinkContext _context;

        public CustomerRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CustomerResponseModel> AddCustomerAsync(CustomerRequestModel Request)
        {
            try
            {
                var customer = new Customer(Request);
                await _context.Customers.InsertOneAsync(customer);
                return new CustomerResponseModel(customer);
            }
            catch (Exception e)
            {
                return new CustomerResponseModel(null, e.Message, true);
            }
        }
        public async Task<long> DeleteCustomerAsync(string Id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, Id);
            var result = await _context.Customers.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<CustomerResponseModel>> GetCustomersAsync()
        {
            try
            {
                var result = await _context.Customers.FindAsync(c => true);
                return ExcludeDeletedCusmers(result.ToEnumerable()).Select(c=>new CustomerResponseModel(c));
            }
            catch (Exception e)
            {
                return new[] { new CustomerResponseModel(null, e.Message, true) };
            }
        }

        public async Task<CustomerResponseModel> GetCustomerAsync(string Id)
        {
            try
            {
                var result = await _context.Customers.FindAsync(c => c.Id == Id && c.State == State.Active);
                return new CustomerResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new CustomerResponseModel(null, e.Message, true);
            }
        }

        public async Task<CustomerResponseModel> UpdateCustomerAsync(CustomerRequestModel request)
        {
            try
            {
                var filter = Builders<Customer>.Filter.Eq(c => c.Id, request.Id);
                var update = Builders<Customer>.Update
                    .Set(c => c.UserId, request.UserId)
                    .Set(c => c.Name, request.Name)
                    .Set(c => c.LastName, request.LastName)
                    .Set(c => c.Email, request.Email)
                    .Set(c => c.PhoneNumber, request.PhoneNumber)
                    .Set(c => c.LocationId, request.LocationId)
                    .Set(c => c.Address, request.Address)
                    .Set(c => c.OrderIds, request.OrderIds)
                    .Set(c => c.ReviewIds, request.ReviewIds)
                    .Set(c => c.State, request.State)
                    .Set(c => c.DeletedAt, request.DeletedAt);

                var options = new FindOneAndUpdateOptions<Customer>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var updatedCustomer = await _context.Customers.FindOneAndUpdateAsync(filter, update, options);
                return new CustomerResponseModel(updatedCustomer);
            }
            catch (Exception e)
            {
                return new CustomerResponseModel(null, e.Message, true);
            }
        }


        public async Task<long> SoftDeleteCustomerAsync(string Id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, Id);
            var update = Builders<Customer>.Update
                .Set(c => c.State, State.Deleted)
                .Set(c => c.DeletedAt, DateTime.UtcNow.ToString());

            var result = await _context.Customers.UpdateOneAsync(filter:filter,update:update);
            return result.ModifiedCount;
        }
        private IEnumerable<Customer> ExcludeDeletedCusmers(IEnumerable<Customer> customers)
        {
            return customers.Where(c => c.State == State.Active).ToList();
        }
    }
}
