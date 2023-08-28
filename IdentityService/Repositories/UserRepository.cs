using FarmLink.IndentityService.Models;
using FarmLink.IndentityService.Models.RequestModels;
using FarmLink.Shared.Enumarations;
using IdentityService.Data.Interfaces;
using IdentityService.Repositories.Interfaces;
using MongoDB.Driver;

namespace IdentityService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFarmLinkContext _context;

        public UserRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var result= await _context.Users.FindAsync(u => true);
            return result.ToEnumerable().Where(x => x.UserState == State.Active);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var result = await _context.Users.FindAsync(u => u.Id == id);
            return result.FirstOrDefault();
        }
        public async Task<User> GetUserByEmailAsync(string Email)
        {
            var result = await _context.Users.FindAsync(u => u.Email == Email);
            return result.FirstOrDefault();
        }

        public async Task<User> LoginAsync(LoginRequestModel User)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, User.Email) &
                         Builders<User>.Filter.Eq(u => u.Password, User.Password);

            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<User> AddUserAsync(UserRequestModel Request)
        {
            var user = new User(Request);
            await _context.Users.InsertOneAsync(user);
            return user;
        }
        public async Task<User> UpdateUserAsync(UserRequestModel updatedUser)
        {
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

            return await _context.Users.FindOneAndUpdateAsync(filter, update, options);
        }
        public async Task<User> UpdateUserAsync(User updatedUser)
        {
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

            return await _context.Users.FindOneAndUpdateAsync(filter, update, options);
        }
        public async Task<bool> SoftDeleteUserAsync(string Id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var update = Builders<User>.Update
                .Set(u => u.UserState, State.Deleted)
                .Set(u => u.DeletedAt, DateTime.UtcNow.ToString());

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            var result = await _context.Users.FindOneAndUpdateAsync(filter, update, options);
            return result.UserState == State.Deleted;
        }
        public async Task<long> DeleteUserAsync(string Id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var result = await _context.Users.DeleteOneAsync(filter);
            return result.DeletedCount;
        }
        public async Task<long> UpdateUserOtpAsync(User User)
        {
            var update = await _context.Users.UpdateOneAsync(y => y.Id == User.Id, Builders<User>.Update.Set(x => x.Otp, User.Otp));
            return update.ModifiedCount;
        }
        public async Task<User> UpdateUserPasswordAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Salt, user.Salt)
                .Set(u => u.Password, user.Password)
                .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString());

            var options = new FindOneAndUpdateOptions<User>
            {
                ReturnDocument = ReturnDocument.After
            };

            var result = await _context.Users.FindOneAndUpdateAsync(filter, update, options);
            return result;
        }
    }
}
