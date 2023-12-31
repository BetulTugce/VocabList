﻿using System.Net;
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
                return (await response.Content.ReadFromJsonAsync<GetRolesToEndpointQueryResponse>(), response.StatusCode);
            }
            catch (Exception)
            {
                return (null, HttpStatusCode.InternalServerError);
            }

        }

        // Request ile gelen rolleri ilgili menunun altındaki code ile işaretlenmiş olan endpointe atar yani, endpointleri rollerle ilişkilendirir.
        public async Task<HttpStatusCode> AssignRoleEndpoint(AssignRoleEndpointRequest request, string accessToken)
        {
            try
            {
                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}AuthorizationEndpoints", request);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }

        }
    }
}
