using VocabList.Core.DTOs;
using VocabList.Core.DTOs.Identity;

namespace VocabList.Core.Authentications
{
    public interface IInternalAuthentication
    {
        Task<LoginUserResponse> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
        Task<LoginUserResponse> RefreshTokenLoginAsync(string refreshToken);
    }
}
