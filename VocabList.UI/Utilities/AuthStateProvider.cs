using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace VocabList.UI.Utilities
{
    //TODO: AccessTokenın expire olmuşsa refresh token ile kullanıcıyı tekrar login etme durumu için düzenlenecek..
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationState anonymous;
        private readonly HttpClient _httpClient;

        // Dependency Injectionla ILocalStorageService ve anonymous AuthenticationState'in başlangıç değeri ayarlanıyor..
        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            anonymous = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));
            _httpClient = httpClient;
        }

        // Kullanıcının kimlik doğrulama durumunu kontrol eder.
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // AccessToken değeri local storageden alınıyor..
            String accessToken = await _localStorageService.GetItemAsStringAsync("AccessToken");
            // Eğer AccessToken bulunamazsa veya boşsa, anonim/misafir AuthenticationState döner.
            if (String.IsNullOrEmpty(accessToken))
            {
                return anonymous;
            }
            // Eğer AccessToken bulunursa, kullanıcı adını local storageden alır ve bu bilgiyi içeren bir AuthenticationState döner.
            String username = await _localStorageService.GetItemAsStringAsync("Username");

            //// Kullanıcı adını içeren bir claim oluşturuluyor..
            //var usernameClaim = new Claim(ClaimTypes.Name, username);
            //// Kullanıcı adı claimini içeren bir ClaimsIdentity oluşturuluyor..
            //var identity = new ClaimsIdentity(new[] { usernameClaim }, "jwtAuthType");
            //// ClaimsIdentity içeren bir ClaimsPrincipal oluşturuluyor..
            //var claimsPrincipal = new ClaimsPrincipal(identity);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "jwtAuthType"));

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return new AuthenticationState(claimsPrincipal);
        }

        // Kullanıcı giriş yaptığında çağrılır.
        public void NotifyUserLogin(String username)
        {
            // Kullanıcının kimlik doğrulama durumunu günceller ve NotifyAuthenticationStateChanged metodu aracılığıyla bu değişikliği bileşenlere bildirir.
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        // Kullanıcı çıkış yaptığında çağrılır.
        public void NotifyUserLogout()
        {
            // Kimlik doğrulama durumunu anonim/misafir AuthenticationState ile günceller ve bu değişikliği bileşenlere bildirir.
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
