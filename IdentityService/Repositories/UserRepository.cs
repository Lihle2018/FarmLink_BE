using FarmLink.IndentityService.Models;
using FarmLink.IndentityService.Models.RequestModels;
using FarmLink.Shared.Enumarations;
using IdentityService.Data.Interfaces;
using IdentityService.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using ReturnDocument = MongoDB.Driver.ReturnDocument;

namespace IdentityService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFarmLinkContext _context;

        public UserRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<UserResponseModel>> GetUsersAsync()
        {
            try
            {
                var result = await _context.Users.FindAsync(u => true);
                return result.ToEnumerable().Where(x => x.UserState == State.Active).Select(u=>new UserResponseModel(u));
            }
            catch (Exception e)
            {
                return new[] { new UserResponseModel(null, e.Message, true) };
            }
        }

        public async Task<UserResponseModel> GetUserByIdAsync(string id)
        {
            try
            {
                var result = await _context.Users.FindAsync(u => u.Id == id);
                return new UserResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
        }

        public async Task<UserResponseModel> GetUserByEmailAsync(string Email)
        {
            try
            {
                var result = await _context.Users.FindAsync(u => u.Email == Email);
                return new UserResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new UserResponseModel(null,e.Message, true);
            }
        }

        public async Task<UserResponseModel> LoginAsync(LoginRequestModel User)
        {
            try
            {

                var filter = Builders<User>.Filter.Eq(u => u.Email, User.Email) &
                             Builders<User>.Filter.Eq(u => u.Password, User.Password);

                var result = await _context.Users.Find(filter).FirstOrDefaultAsync();
                return new UserResponseModel(result);
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
        }

        public async Task<UserResponseModel> AddUserAsync(UserRequestModel Request)
        {
            try
            {
                var user = new User(Request);
                await _context.Users.InsertOneAsync(user);
                return new UserResponseModel(user);
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
        }

        public async Task<UserResponseModel> UpdateUserAsync(UserRequestModel updatedUser)
        {
            try
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
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var result= await _context.Users.FindOneAndUpdateAsync(filter, update, options);
                return new UserResponseModel(result);
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
        }

        public async Task<UserResponseModel> UpdateUserAsync(User updatedUser)
        {
            try
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
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var result = await _context.Users.FindOneAndUpdateAsync(filter, update, options);
                return new UserResponseModel(result);
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
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

        public async Task<UserResponseModel> UpdateUserPasswordAsync(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                var update = Builders<User>.Update
                    .Set(u => u.Salt, user.Salt)
                    .Set(u => u.Password, user.Password)
                    .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString());

                var options = new FindOneAndUpdateOptions<User>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert=false
                };

                var result = await _context.Users.FindOneAndUpdateAsync(filter, update, options);
                return new UserResponseModel(result);
            }
            catch (Exception e)
            {
                return new UserResponseModel(null, e.Message, true);
            }
        }
    }
}
