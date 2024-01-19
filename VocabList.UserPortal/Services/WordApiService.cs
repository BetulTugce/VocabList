using System.Net;
using VocabList.UserPortal.Data.Words;

namespace VocabList.UserPortal.Services
{
    public class WordApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public WordApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // UserId ve wordList id bilgisine göre ilgili kullanıcı için bir kelime oluşturur..
        public async Task<HttpStatusCode> CreateWordAsync(CreateWordRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Words", request);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        // Kelimeyi güncellemek için kulanılır..
        public async Task<HttpStatusCode> UpdateWordAsync(Word request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // İlgili URL'ye verilen model verilerine sahip bir JSON içeriği gönderiliyor.
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Words", request);
            return response.StatusCode;
        }

        // Kelimeyi silmek için kullanılır..
        public async Task<HttpStatusCode> DeleteWordAsync(int id, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}Words/{id}");
            return response.StatusCode;
        }

        // UserIdnin oluşturduğu kelimeleri page ve size parametrelerine göre liste halinde getiriyor..
        public async Task<(List<Word>, HttpStatusCode)> GetAllWordsByUserIdAndWordListIdAsync(GetAllWordsByUserIdAndWordListIdRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Words/GetAllByUserIdAndWordListId", request);

                if (response.IsSuccessStatusCode)
                {
                    List<Word> data = await response.Content.ReadFromJsonAsync<List<Word>>();
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

        public async Task<(int, HttpStatusCode)> GetTotalCountByWordListId(GetTotalCountByWordListIdRequest request, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Words/GetTotalCountByWordListId", request);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<int>();
                    return (data, response.StatusCode);
                }

                return (0, response.StatusCode);
            }
            catch (Exception)
            {
                return (0, HttpStatusCode.InternalServerError);
            }
        }
    }
}
