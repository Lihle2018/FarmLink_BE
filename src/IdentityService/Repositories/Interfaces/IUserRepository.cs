using FarmLink.IndentityService.Models.RequestModels;
using FarmLink.IndentityService.Models;

namespace IdentityService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponseModel>> GetUsersAsync();
        Task<UserResponseModel> GetUserByIdAsync(string id);
        Task<UserResponseModel> GetUserByEmailAsync(string Email);
        Task<UserResponseModel> AddUserAsync(UserRequestModel Request);
        Task<UserResponseModel> UpdateUserAsync(UserRequestModel updatedUser);
        Task<UserResponseModel> UpdateUserAsync(User updatedUser);
        Task<long> DeleteUserAsync(string Id);
        Task<bool> SoftDeleteUserAsync(string Id);
        Task<UserResponseModel> LoginAsync(LoginRequestModel User);
        Task<long> UpdateUserOtpAsync(User User);
        Task<UserResponseModel> UpdateUserPasswordAsync(User user);
    }
}
