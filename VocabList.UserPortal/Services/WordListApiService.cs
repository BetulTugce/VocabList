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

        // UserId bilgisine göre ilgili kullanıcı için bir liste oluşturur..
        public async Task<HttpStatusCode> CreateWordListAsync(CreateWordListRequest request, string accessToken)
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

        // UserIdnin oluşturduğu listeleri page ve size parametrelerine göre liste halinde getiriyor..
        public async Task<(List<WordList>, HttpStatusCode)> GetAllWordListsByUserIdAsync(GetAllWordListsByUserIdRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //var url = $"{_baseUrl}WordLists?Page={request.Page}&Size={request.Size}&AppUserId={request.AppUserId}";
                //var response = await _httpClient.GetAsync(url);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}WordLists/GetAllByUserId", request);
                
                if (response.IsSuccessStatusCode)
                {
                    List<WordList> data = await response.Content.ReadFromJsonAsync<List<WordList>>();
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
