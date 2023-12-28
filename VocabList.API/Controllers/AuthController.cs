using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Token;

namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUser model)
        {
            LoginUserResponse response = new LoginUserResponse();
            response = await _authService.LoginAsync(model.UsernameOrEmail, model.Password, 900);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLogin refreshTokenLogin)
        {
            Token token = await _authService.RefreshTokenLoginAsync(refreshTokenLogin.RefreshToken);
            LoginUserResponse response = new()
            {
                Token = token
            };
            return Ok(response);
        }

        // Bu endpoint, kullanıcıdan email bilgisini alıp veritabanında ilgili kullanıcıyı bulursa ResetToken oluşturarak kullanıcıya parola değişikliği için mail gönderiyor..
        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(string email)
        {
            await _authService.PasswordResetAsnyc(email);
            return Ok();
        }

        // Kullanıcı veritabanında kayıtlı ise ResetTokenı decode ederek doğrulama işlemini yürütüyor..
        [HttpPost("verify-reset-token")]
        public async Task<VerifyPasswordResetTokenResponse> VerifyResetToken([FromBody] VerifyPasswordResetToken request)
        {
            bool state = await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);
            return new()
            {
                State = state,
            };
        }
    }
}
