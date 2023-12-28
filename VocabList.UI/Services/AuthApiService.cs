using VocabList.UI.Data;
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
