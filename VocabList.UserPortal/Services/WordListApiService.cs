using System.Net;
using System.Net.Http.Json;
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

        // Kelime listesini güncellemek için kulanılır..
        public async Task<HttpStatusCode> UpdateWordListAsync(WordList request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // İlgili URL'ye verilen model verilerine sahip bir JSON içeriği gönderiliyor.
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}WordLists", request);
            return response.StatusCode;
        }

        // Kelime listesini silmek için kullanılır..
        //public async Task<bool> DeleteWordListAsync(int id, string accessToken)
        public async Task<HttpStatusCode> DeleteWordListAsync(int id, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}WordLists/{id}");
            return response.StatusCode;
        }

        // Kelime listesini idye göre getirir..
        public async Task<(WordList, HttpStatusCode)> GetWordListByIdAsync(int id, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"{_baseUrl}WordLists/{id}");
                var responseWordList = await response.Content.ReadFromJsonAsync<WordList>();
                return (responseWordList, response.StatusCode);
            }
            catch (Exception)
            {
                return (null, HttpStatusCode.InternalServerError);
            }
        }
        
        // Kelime listesini idye ve userIdye göre getirir..
        public async Task<(WordList, HttpStatusCode)> GetWordListByIdAndUserIdAsync(GetWordListByIdAndUserIdRequest request, string accessToken)
        {
            try
            {
                // AccessToken ile Authorization başlığı ayarlanıyor..
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}WordLists/GetByIdAndUserId", request);
                if (response.IsSuccessStatusCode)
                {
                    var responseWordList = await response.Content.ReadFromJsonAsync<WordList>();
                    return (responseWordList, response.StatusCode);
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

        public async Task<(int, HttpStatusCode)> GetTotalCountByUserId(GetTotalCountByUserIdRequest request, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}WordLists/GetTotalCountByUserId", request);
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
