using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;

namespace VocabList.Core.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task<CreateUserResponse> GetUserByIdAsync(string userId);
        Task<CreateUserResponse> GetUserByNameorEmailAsync(string userName);
        //Task<List<CreateUserResponse>> GetAllUsersAsync();
        //Task<List<ListUser>> GetAllUsersAsync(int page, int size);
        Task<List<CreateUserResponse>> GetAllUsersAsync(int page, int size);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        //int TotalUsersCount { get; }
        Task<int> GetTotalCountAsync();
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
        Task<(bool, string?)> UpdateUserProfileImageAsync(string userId, string path);
        //Task<(string, CreateUserResponse)> GetPofileImageByUserIdAsync(string userId);
        Task<string> GetPofileImageByUserIdAsync(string userId);
    }
}
