using VocabList.Core.DTOs;

namespace VocabList.Core.Authentications
{
    public interface IInternalAuthentication
    {
        Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
        Task<Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
