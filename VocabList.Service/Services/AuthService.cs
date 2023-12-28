using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Token;
using VocabList.Service.Helpers;
using VocabList.Service.Mail;

namespace VocabList.Service.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<AppUser> _signInManager;
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }

        // Username/e-posta ve parola ile giriş yapmak için..
        public async Task<LoginUserResponse> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
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

                LoginUserResponse response = new()
                {
                    Token = token,
                    User = MapToUserDto(user)
                };
                return response;
                
            }
            throw new Exception("Giriş başarısız! Lütfen bilgilerinizi kontrol edin.");
        }

        public CreateUserResponse MapToUserDto(AppUser user)
        {
            return new CreateUserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        // Kullanıcı veritabanında kayıtlı ise bir ResetToken oluşturup kullanıcıya mail atacak..
        public async Task PasswordResetAsnyc(string email)
        {
            // Gelen email ile bir kullanıcı kayıtlı ise..
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // ResetToken oluşturuluyor..
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                #region Encode
                // ResetToken byte dönüştürülüp diziye atanıyor..
                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
                // Reset token urlde taşınabilir bir stringe dönüştürerek şifreliyor..
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
                #endregion

                // Token urlde taşınabilir bir stringe dönüştürülerek şifreleniyor, kullanıcı kendine gelen maille ilgili linke tıkladığında bu token decode edilecek ve doğrulanacak..
                resetToken = resetToken.UrlEncode();

                // Kullanıcıya parola değişikliği yapabilmesi için mail gönderiliyor..
                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
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

        // Kullanıcının parola değişikliği için ResetTokenı doğrulayacak olan method..
        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            // UserId veritabanında kayıtlı mı kontrol ediliyor..
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null) // Kullanıcı bulunduysa..
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                
                // ResetToken decode ediliyor..
                resetToken = resetToken.UrlDecode();

                // ResetToken doğrulanıyor ve true ya da false şeklinde ilgili değeri dönüyor..
                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }

            // Kullanıcı bulunamadıysa false dönüyor.
            return false;
        }
    }
}
