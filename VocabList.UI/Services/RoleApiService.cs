using System.Net;
using VocabList.UI.Data.Roles;
using VocabList.UI.Data.Users;

namespace VocabList.UI.Services
{
    public class RoleApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public RoleApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            //_baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        // Rolleri (page ve size parametrelerine göre) getirir..
        public async Task<(GetRolesQueryResponse, HttpStatusCode)> GetAllRoles(string accessToken, GetRolesQueryRequest request)
        {
            try
            {
                //string apiUrl = $"{_baseUrl}/authorize-definition-endpoints";
                //String accessToken = await _localStorageService.GetItemAsStringAsync("AccessToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var queryString = $"?Page={request.Page}&Size={request.Size}";
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}Roles{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<GetRolesQueryResponse>();
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

        // Rol eklemek için kullanılır..
        public async Task<bool> CreateRoleAsync(CreateRoleRequest request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Roles", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Rol silmek için kullanılır..
        public async Task<bool> DeleteRoleAsync(DeleteRoleRequest request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}Roles/{request.Id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Rol güncellemek için kulanılır..
        public async Task<bool> UpdateRoleAsync(UpdateRoleRequest request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // İlgili URL'ye verilen model verilerine sahip bir JSON içeriği gönderiliyor.
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Roles/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Toplam rol sayısını döner..
        public async Task<(int, HttpStatusCode)> GetTotalCount(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"{_baseUrl}Roles/GetTotalCount");
                var data = await response.Content.ReadFromJsonAsync<int>();
                return (data, response.StatusCode);
            }
            catch (Exception)
            {
                return (0, HttpStatusCode.InternalServerError);
            }
        }
    }
}
