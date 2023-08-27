using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;

namespace Infrastructure.Repositories.Interfaces
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
}
