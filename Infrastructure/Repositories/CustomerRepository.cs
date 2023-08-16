using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories
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
    public class CustomerRepository : ICustomerRepository
    {
        IMongoCollection<Customer> _customers;
        private readonly IConfiguration _configuration;

        private readonly IOptions<DatabaseSettings> _config;

        public CustomerRepository(IOptions<DatabaseSettings> config, IConfiguration configuration)
        {
            _config = config;

            _configuration = configuration;
        }

        public bool Connect()
        {
            try
            {
                var t = _config.Value;
                var client = new MongoClient(_configuration.GetSection("CustomersTableConnection").Value);
                var database = client.GetDatabase(_configuration.GetSection("CustomersTableName").Value);

                _customers = database.GetCollection<Customer>(_configuration.GetSection("CustomersCollectionName").Value);
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Customer> AddCustomerAsync(CustomerRequestModel Request)
        {
            Connect();
            var customer = new Customer(Request);
            await _customers.InsertOneAsync(customer);
            return customer;

        }
        public async Task<long> DeleteCustomerAsync(string Id)
        {
            Connect();
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, Id);
            var result = await _customers.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            Connect();
            return ExcludeDeletedCusmers((await _customers.FindAsync(c => true)).ToList());
        }

        public async Task<Customer> GetCustomerByIdAsync(string Id)
        {
            Connect();
            var result = await _customers.FindAsync(c => c.Id == Id&&c.State==State.Active);
            return result.FirstOrDefault();
        }

        public async Task<Customer> UpdateCustomerAsync(CustomerRequestModel request)
        {
            Connect();
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
                ReturnDocument = ReturnDocument.After
            };

            var updatedCustomer = await _customers.FindOneAndUpdateAsync(filter, update, options);
            return updatedCustomer;
        }



        public async Task<long> SoftDeleteCustomerAsync(string Id)
        {
            Connect();
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, Id);
            var update = Builders<Customer>.Update
                .Set(c => c.State, State.Deleted)
                .Set(c => c.DeletedAt, DateTime.UtcNow.ToString());

            var result = await _customers.UpdateOneAsync(filter, update);
            return result.ModifiedCount;
        }
        private List<Customer> ExcludeDeletedCusmers(List<Customer> customers)
        {
            return customers.Where(c => c.State == State.Active).ToList();
        }
    }
}
