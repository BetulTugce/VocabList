using System.Text;
using VocabList.UI.Data.Users;

namespace VocabList.UI.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest model)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Auth/Login", model);
            if (response.IsSuccessStatusCode)
            {
                ////HTTP yanıtının başarı durum kodu içerip içermediği kontrol edilir..
                //response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<LoginUserResponse>();
            }
            else
            {
                return null;
            }

        }

        // Parola sıfırlamak için kullanıcıdan email bilgisi alınarak ilgili actiona post isteği atılıyor. Eğer istek başarılı olursa girilen e-posta adresine parola sıfırlamak için bir link gönderiliyor..
        public async Task<bool> PasswordReset(ForgotPasswordRequest request)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Auth/password-reset", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Parola sıfırlama linkine tıklandığında resetTokenı doğrulayacak olan actiona istek atıyor.. ResetToken doğrulanırsa kullanıcı parolasını güncellemek için ilgili sayfada yeni parolasını girebilecek. UsersControllerdaki UpdatePassword actionına istek atılarak işlem tamamlanacak..
        public async Task<VerifyPasswordResetTokenResponse> VerifyResetTokenAsync(VerifyPasswordResetTokenRequest request)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Auth/verify-reset-token", request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<VerifyPasswordResetTokenResponse>();
                return new()
                {
                    State = data.State,
                };
            }
            else
            {
                return new()
                {
                    State = false
                };
            }

        }

        public async Task<LoginUserResponse> RefreshTokenLoginAsync(RefreshTokenLoginRequest model)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Auth/RefreshTokenLogin", model);
            if (response.IsSuccessStatusCode)
            {
                ////HTTP yanıtının başarı durum kodu içerip içermediği kontrol edilir..
                //response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<LoginUserResponse>();
            }
            else
            {
                return null;
            }

        }
    }
}
