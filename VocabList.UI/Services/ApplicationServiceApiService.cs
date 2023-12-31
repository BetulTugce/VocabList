using Blazored.LocalStorage;
using System.Net;
using VocabList.UI.Data.ApplicationServices;

namespace VocabList.UI.Services
{
	public class ApplicationServiceApiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl;
        private readonly ILocalStorageService _localStorageService;

        public ApplicationServiceApiService(HttpClient httpClient, IConfiguration configuration, ILocalStorageService localStorageService)
		{
			_httpClient = httpClient;
			_baseUrl = configuration["ApiSettings:BaseUrl"];
			_localStorageService = localStorageService;
		}

		public async Task<(List<GetAuthorizeDefinitionEndpointsResponse>, HttpStatusCode)> GetAuthorizeDefinitionEndpoints(string accessToken)
		{
			try
			{
				//string apiUrl = $"{_baseUrl}/authorize-definition-endpoints";
				//String accessToken = await _localStorageService.GetItemAsStringAsync("AccessToken");
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
				HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}ApplicationServices");

				if (response.IsSuccessStatusCode)
				{
                    var list = await response.Content.ReadFromJsonAsync<List<GetAuthorizeDefinitionEndpointsResponse>>();
                    return (list, response.StatusCode);
                }
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					return (new List<GetAuthorizeDefinitionEndpointsResponse>(), HttpStatusCode.Unauthorized);
				}
				else
				{
                    return (new List<GetAuthorizeDefinitionEndpointsResponse>(), HttpStatusCode.BadRequest);
                }
            }
			catch (Exception ex)
			{
				return (new List<GetAuthorizeDefinitionEndpointsResponse>(), HttpStatusCode.InternalServerError);
			}
		}
	}
}
