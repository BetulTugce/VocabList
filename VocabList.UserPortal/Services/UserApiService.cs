using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using VocabList.UserPortal.Data.Users;

namespace VocabList.UserPortal.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UserApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            //_baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest model)
        {
            //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Users", model);
            if (response.IsSuccessStatusCode)
            {
                ////HTTP yanıtının başarı durum kodu içerip içermediği kontrol edilir..
                //response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            }
            else if (response.StatusCode == HttpStatusCode.Conflict) // HTTP durumu 409 (Conflict) ise
            {
                // Hata durumunu kontrol etmek için özel bir nesne döndürür..
                return new CreateUserResponse { IsConflictError = true };
            }
            else
            {
                return null;
            }

        }

        //public async Task<(UploadProfileImageResponse,HttpStatusCode)> UploadFileAsync(IBrowserFile file)
        public async Task<(UploadProfileImageResponse, HttpStatusCode)> UploadFileAsync(MultipartFormDataContent content, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.PostAsync($"{_baseUrl}Users/fileupload", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UploadProfileImageResponse>();
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

        // Kullanıcının profil fotoğrafını güncellemek için (ProfileImagePath kolonunu) kullanılır..
        public async Task<HttpStatusCode> UpdateProfileImageAsync(UpdateProfileImageRequest request, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // İlgili URL'ye verilen model verilerine sahip bir JSON içeriği gönderiliyor.
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Users/update-profile-image", request);
            return response.StatusCode;
        }

        // Kullanıcının id bilgisi parametre olarak kullanılıyor ve profil resmi alınıyor..
        public async Task<(byte[], HttpStatusCode)> GetProfileImageByUserIdAsync(string userId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{_baseUrl}Users/profile-image/{userId}");
            // Başarılı bir şekilde cevap alındıysa..
            if (response.IsSuccessStatusCode)
            {
                // İçerik okunuyor..
                var contentStream = await response.Content.ReadAsStreamAsync();

                // İçeriği byte dizisine dönüştürüyor..
                var imageBytes = new byte[contentStream.Length];
                await contentStream.ReadAsync(imageBytes, 0, imageBytes.Length);

                return (imageBytes, response.StatusCode);
            }
            return (null, response.StatusCode);
        }

        public async Task<(User user, HttpStatusCode statusCode)> GetUserByIdAsync(string id, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"{_baseUrl}Users/{id}");
                var user = await response.Content.ReadFromJsonAsync<User>();
                return (user, response.StatusCode);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Exception: {ex.Message}");
                return (null, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpStatusCode> UpdatePasswordAsync(UpdatePasswordRequest model)
        {
            try
            {
                //İlgili urle verilen model verilerine sahip bir json içeriği gönderiliyor..
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Users/update-password", model);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
