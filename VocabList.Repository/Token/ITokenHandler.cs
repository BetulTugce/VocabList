using VocabList.Core.Entities.Identity;

namespace VocabList.Repository.Token
{
    public interface ITokenHandler
    {
        // Belirli bir kullanıcı için geçerli süre içinde bir token oluşturur.
        Core.DTOs.Token CreateAccessToken(int second, AppUser appUser);

        // AccessToken geçerliliğini kaybettiğinde kullanılarak yeni bir token alınmasını sağlar.
        string CreateRefreshToken();
    }
}
