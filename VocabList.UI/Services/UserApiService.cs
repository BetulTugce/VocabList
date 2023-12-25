using System.Net;
using System.Text;
using VocabList.UI.Data;
using static System.Net.WebRequestMethods;

namespace VocabList.UI.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UserApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            //_baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest model)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Users", model);
            if (response.IsSuccessStatusCode)
            {
                ////HTTP yanıtının başarı durum kodu içerip içermediği kontrol edilir..
                //response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            }
            else if (response.StatusCode == HttpStatusCode.Conflict) // HTTP durumu 409 (Conflict) ise
            {
                // Hata durumunu kontrol etmek için özel bir nesne döndürür..
                return new CreateUserResponse { IsConflictError = true };
            }
            else
            {
                return null;
            }

        }
    }
}
