using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool Connect();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string Email);
        Task<User> AddUserAsync(UserRequestModel Request);
        Task<User> UpdateUserAsync(UserRequestModel updatedUser);
        Task<User> UpdateUserAsync(User updatedUser);
        Task<long> DeleteUserAsync(string Id);
        Task<bool> SoftDeleteUserAsync(string Id);
        Task<User> LoginAsync(LoginRequestModel User);
        Task<long> UpdateUserOtpAsync(User User);
        Task<User> UpdateUserPasswordAsync(User user);
    }
    public class UserRepository : IUserRepository
    {
        IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;

        private readonly IOptions<DatabaseSettings> _config;

        public UserRepository(IOptions<DatabaseSettings> config, IConfiguration configuration)
        {
            _config = config;

            _configuration = configuration;
        }

        public bool Connect()
        {
            try
            {
                var t = _config.Value;
                var client = new MongoClient(_configuration.GetSection("UsersTableConnection").Value);
                var database = client.GetDatabase(_configuration.GetSection("UsersTableName").Value);

                _users = database.GetCollection<User>(_configuration.GetSection("UsersCollectionName").Value);
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            Connect();
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            Connect();
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByEmailAsync(string Email)
        {
            Connect();
            return await _users.Find(user => user.Email == Email).FirstOrDefaultAsync();
        }

        public async Task<User> LoginAsync(LoginRequestModel User)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Email, User.Email) &
                         Builders<User>.Filter.Eq(u => u.Password, User.Password);

            return await _users.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<User> AddUserAsync(UserRequestModel Request)
        {
            Connect();
            var user = new User(Request);
            await _users.InsertOneAsync(user);
            return user;
        }
        public async Task<User> UpdateUserAsync(UserRequestModel updatedUser)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Id, updatedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
                .Set(u => u.Role, updatedUser.Role)
                .Set(u => u.ProfilePicture, updatedUser.ProfilePicture)
                .Set(u => u.IsEmailVerified, updatedUser.IsEmailVerified)
                .Set(u => u.IsPhoneVerified, updatedUser.IsPhoneVerified)
                .Set(u => u.Otp, updatedUser.Otp)
                .Set(u => u.Salt, updatedUser.Salt)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString())
                 .Set(u => u.UserState, updatedUser.UserState);

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _users.FindOneAndUpdateAsync(filter, update, options);
        }
        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Id, updatedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
                .Set(u => u.Role, updatedUser.Role)
                .Set(u => u.ProfilePicture, updatedUser.ProfilePicture)
                .Set(u => u.IsEmailVerified, updatedUser.IsEmailVerified)
                .Set(u => u.IsPhoneVerified, updatedUser.IsPhoneVerified)
                .Set(u => u.Otp, updatedUser.Otp)
                .Set(u => u.Salt, updatedUser.Salt)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString())
                .Set(u => u.UserState, updatedUser.UserState);

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _users.FindOneAndUpdateAsync(filter, update, options);
        }
        public async Task<bool> SoftDeleteUserAsync(string Id)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var update = Builders<User>.Update
                .Set(u => u.UserState, State.Deleted)
                .Set(u => u.DeletedAt, DateTime.UtcNow.ToString());

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            var result = await _users.FindOneAndUpdateAsync(filter, update, options);
            return result.UserState == State.Deleted;
        }
        public async Task<long> DeleteUserAsync(string Id)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var result = await _users.DeleteOneAsync(filter);
            return result.DeletedCount;
        }
        public async Task<long> UpdateUserOtpAsync(User User)
        {
            Connect();
            var update = await _users.UpdateOneAsync(y => y.Id == User.Id, Builders<User>.Update.Set(x => x.Otp, User.Otp));
            return update.ModifiedCount;
        }
        public async Task<User> UpdateUserPasswordAsync(User user)
        {
            Connect();
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Salt, user.Salt)
                .Set(u => u.Password, user.Password)
                .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString());

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            var result = await _users.FindOneAndUpdateAsync(filter, update, options);
            return result;
        }
    }
}
