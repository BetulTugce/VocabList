using System.Net;
using VocabList.UI.Data.AuthorizationEndpoints;

namespace VocabList.UI.Services
{
    public class AuthorizationEndpointApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthorizationEndpointApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // Endpointlere atanmış rolleri getiriyor..
        public async Task<(GetRolesToEndpointQueryResponse, HttpStatusCode)> GetRolesToEndpointAsync(GetRolesToEndpointQueryRequest request, string accessToken)
        {
            try
            {
                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}AuthorizationEndpoints/GetRolesToEndpoint", request);
                return (await response.Content.ReadFromJsonAsync<GetRolesToEndpointQueryResponse>(), HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return (null, HttpStatusCode.InternalServerError);
            }

        }
    }
}
