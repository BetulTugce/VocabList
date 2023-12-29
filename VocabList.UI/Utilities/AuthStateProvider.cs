using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using System.Security.Claims;
using VocabList.UI.Data.Users;
using VocabList.UI.Services;

namespace VocabList.UI.Utilities
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationState anonymous;
        private readonly HttpClient _httpClient;
        private readonly AuthApiService _authApiService;

        // Dependency Injectionla ILocalStorageService ve anonymous AuthenticationState'in başlangıç değeri ayarlanıyor..
        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient, AuthApiService authApiService)
        {
            _localStorageService = localStorageService;
            anonymous = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));
            _httpClient = httpClient;
            _authApiService = authApiService;
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

            // Access Token'ın süresi dolmuşsa Refresh Token kullanarak otomatik olarak yeniden kimlik doğrulama yapar..
            if (await IsAccessTokenExpired(accessToken))
            { // Eğer süre dolmuşsa (true ise) RefreshToken aracılığı ile tekrar giriş yapılır ve yeni token alınır..
                await RefreshTokenAsync();
            }

            return new AuthenticationState(claimsPrincipal);
        }

        // AccessTokenın expire edilme süresi ile şuanki zaman karşılaştırılarak geçerliliğini yitirip yitirmediği kontrol ediliyor..
        // True : AccessTokenın ömrü bitmiş.
        // False : AccessToken hala kullanılabilir.
        private async Task<bool> IsAccessTokenExpired(string accessToken)
        {
            // AccessTokenın expire edileceği zaman localstoragedan alınıyor ve şuanki zaman ile kıyas ediliyor..
            var expirationString = await _localStorageService.GetItemAsStringAsync("AccessTokenExpiration");

            if (expirationString is not null)
            {
                // DateTime doğru formata çevriliyor..
                expirationString = expirationString.Replace("T", " ");
                expirationString = expirationString.Replace("Z", " ");
                expirationString = expirationString.Replace("\"", "");
                // Şuanki zaman expirationdan büyükse accesstokenın süresi dolmuştur, true döner..
                return DateTime.UtcNow > DateTime.Parse(expirationString);
            }
            return true;
        }

        public async Task RefreshTokenAsync()
        {
            // LocalStorageden RefreshToken değeri alınıyor..
            String refreshToken = await _localStorageService.GetItemAsStringAsync("RefreshToken");

            // Eğer RefreshToken bulunamazsa veya boşsa, kullanıcı çıkışı yapılır..
            if (String.IsNullOrEmpty(refreshToken))
            {
                NotifyUserLogout();
                return;
            }

            // Bu işlemi yapma sebebim => refreshTokenın başına ve sonuna \" ekleniyor, Substring ile eklenen kısımlardan arındırılıyor..
            // Örneğin OzSfrmQp7MUL3lJCpz\u002ByWpmXipCLSWtOCV7/hTX9LAs= böyle olması gerekirken \"OzSfrmQp7MUL3lJCpz\u002ByWpmXipCLSWtOCV7/hTX9LAs=\" oluyor..
            refreshToken = refreshToken.Substring(1, (refreshToken.Length - 2));

            // RefreshAccessTokenAsync methodu ile AuthApiService üzerinden RefreshTokenLogin actionına istek atılarak sisteme tekrar giriş yapılmaya çalışır..
            var result = await RefreshAccessTokenAsync(refreshToken);

            // Tekrar giriş yapıldıysa..
            if (result is not null)
            {
                // Localstoragedan username bilgisi alınır..
                String username = await _localStorageService.GetItemAsStringAsync("Username");
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "jwtAuthType"));
                var authState = new AuthenticationState(claimsPrincipal);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                // Yeni AccessTokenı, AccessTokenın expire süresini, RefreshTokenı ve usernamei localstoragede günceller..
                await _localStorageService.SetItemAsync("AccessToken", result.Token.AccessToken);
                await _localStorageService.SetItemAsync("AccessTokenExpiration", result.Token.Expiration);
                await _localStorageService.SetItemAsync("RefreshToken", result.Token.RefreshToken);
                await _localStorageService.SetItemAsync("Username", result.User.Username);
            }
            else
            {
                await _localStorageService.ClearAsync();
                NotifyUserLogout();
            }
        }

        private async Task<LoginUserResponse> RefreshAccessTokenAsync(string refreshToken)
        {
            // Servis çağrısı ile Refresh Token kullanarak yeni bir Access Token alınıyor..
            var result = await _authApiService.RefreshTokenLoginAsync(new() { RefreshToken = refreshToken});

            return result;
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
