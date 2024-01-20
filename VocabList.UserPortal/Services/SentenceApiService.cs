using System.Net;
using VocabList.UserPortal.Data.Sentences;

namespace VocabList.UserPortal.Services
{
    public class SentenceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SentenceApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // WordId bilgisine göre ilgili Word için bir cümle ekler..
        public async Task<HttpStatusCode> CreateSentenceAsync(CreateSentenceRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Sentences", request);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        // Cümleyi silmek için kullanılır..
        public async Task<HttpStatusCode> DeleteSentenceAsync(int id, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}Sentences/{id}");
            return response.StatusCode;
        }

        // İlgili idye sahip kelimenin cümlelerini getirir..
        public async Task<(List<Sentence>, HttpStatusCode)> GetSentencesByWordIdAsync(int id, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"{_baseUrl}Sentences/{id}");
                if (response.IsSuccessStatusCode)
                {
                    List<Sentence> data = await response.Content.ReadFromJsonAsync<List<Sentence>>();
                    return (data, response.StatusCode);
                }
                else
                {
                    return (null, response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, HttpStatusCode.InternalServerError);
            }
        }

        // Cümleyi güncellemek için kulanılır..
        public async Task<HttpStatusCode> UpdateSentenceAsync(Sentence request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // İlgili URL'ye verilen model verilerine sahip bir JSON içeriği gönderiliyor.
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Sentences", request);
            return response.StatusCode;
        }
    }
}
