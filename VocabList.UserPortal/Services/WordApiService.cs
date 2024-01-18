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

        // UserId ve wordList id bilgisine göre ilgili kullanıcı için bir liste oluşturur..
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
    }
}
