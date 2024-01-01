using System.Net;
using VocabList.UI.Data.Roles;

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
    }
}
