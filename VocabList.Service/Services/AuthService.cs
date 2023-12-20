using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Token;

namespace VocabList.Service.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<AppUser> _signInManager;
        readonly IUserService _userService;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }

        // Username/e-posta ve parola ile giriş yapmak için..
        public async Task<Core.DTOs.Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            // Önce kullanıcı adı o yoksa e-posta ile kimlik doğrulama yapılıyor.
            AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            // Kullanıcı bulunduysa parola doğrulama işlemi yapılıyor.
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded) // Authentication başarılıysa..
            {
                // Token oluşturuluyor.
                Core.DTOs.Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);

                // User tablosundaki RefreshToken ve bu tokenın expire edileceği zaman güncelleniyor..
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);

                return token;
            }
            throw new Exception("Giriş başarısız! Lütfen bilgilerinizi kontrol edin.");
        }

        // RefreshToken ile giriş yapmak için..
        public async Task<Core.DTOs.Token> RefreshTokenLoginAsync(string refreshToken)
        {
            // İlgili RefreshTokena sahip kullanıcı aranıyor..
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            // Eğer kullanıcı bulunmuşsa ve RefreshToken expire olmamışsa..
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                // Token oluşturuluyor.
                Core.DTOs.Token token = _tokenHandler.CreateAccessToken(15, user);

                // User tablosundaki RefreshToken ve RefreshTokenın expire edileceği zaman oluşturulan token bilgisinden alınarak güncelleniyor..
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else // İlgili RefreshTokena sahip kullanıcı bulunamamışsa veya RefreshToken expire olmuşsa hata verir..
                throw new Exception("Lütfen tekrar giriş yapın!");
        }
    }
}
