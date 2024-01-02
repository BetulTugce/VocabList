using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using VocabList.UI.Data.Users;
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

        // Userları (page ve size parametrelerine göre) getirir..
        public async Task<(GetAllUsersQueryResponse, HttpStatusCode)> GetAllUsers(string accessToken, GetAllUsersQueryRequest request)
        {
            try
            {
                //string apiUrl = $"{_baseUrl}/authorize-definition-endpoints";
                //String accessToken = await _localStorageService.GetItemAsStringAsync("AccessToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var queryString = $"?Page={request.Page}&Size={request.Size}";
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}Users{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<GetAllUsersQueryResponse>();
                    return (data, response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return (null, HttpStatusCode.Unauthorized);
                }
                else
                {
                    return (null, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return (null, HttpStatusCode.InternalServerError);
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

        public async Task<(string[], HttpStatusCode)> GetRolesToUserAsync(string userIdOrName, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                // HTTP GET isteği gönderiliyor..
                var response = await _httpClient.GetAsync($"{_baseUrl}Users/get-roles-to-user/{userIdOrName}");

                // Başarı durumunda rolleri içeren dizi okunuyor ve status codeu ile beraber tuple dönüyor..
                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadFromJsonAsync<string[]>(), response.StatusCode);
                }
                else
                {
                    // Diğer durumlar..
                    return (null, response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, HttpStatusCode.InternalServerError);
            }
        }

        // Requestteki userid bilgisine göre ilgili kullanıcıya seçili rolleri atar..
        public async Task<HttpStatusCode> AssignRoleToUserAsync(AssignRoleToUserRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Users/assign-role-to-user", request);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
