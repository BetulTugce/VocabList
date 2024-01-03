using System.Net;
using VocabList.UserPortal.Data.WordLists;

namespace VocabList.UserPortal.Services
{
    public class WordListApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public WordListApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // Requestteki userid bilgisine göre ilgili kullanıcıya seçili rolleri atar..
        public async Task<HttpStatusCode> CreateWordListAsync(WordList request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}WordLists", request);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
