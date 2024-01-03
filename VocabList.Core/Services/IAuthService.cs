using VocabList.Core.Authentications;

namespace VocabList.Core.Services
{
    public interface IAuthService : IInternalAuthentication
    {
        // Kullanıcı veritabanında kayıtlı ise bir ResetToken oluşturup kullanıcıya parola değiştirme talebi için mail atacak..
        Task PasswordResetAsnyc(string email, bool IsUserPortal);

        // Kullanıcının parola değişikliği için ResetTokenı doğrulayacak olan method..
        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
