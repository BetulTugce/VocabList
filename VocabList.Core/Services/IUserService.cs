using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;

namespace VocabList.Core.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task<CreateUserResponse> GetUserByIdAsync(string userId);
        Task<List<CreateUserResponse>> GetAllUsersAsync();
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
    }
}
