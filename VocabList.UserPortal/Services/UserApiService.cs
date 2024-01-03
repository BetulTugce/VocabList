using System.Net;
using VocabList.UserPortal.Data.Users;

namespace VocabList.UserPortal.Services
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

        public async Task<(User user, HttpStatusCode statusCode)> GetUserByIdAsync(string id, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"{_baseUrl}Users/{id}");
                var user = await response.Content.ReadFromJsonAsync<User>();
                return (user, response.StatusCode);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Exception: {ex.Message}");
                return (null, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpStatusCode> UpdatePasswordAsync(UpdatePasswordRequest model)
        {
            try
            {
                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Users/update-password", model);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
