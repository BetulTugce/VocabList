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
            Token token = await _authService.LoginAsync(model.UsernameOrEmail, model.Password, 900);
            LoginUserResponse response = new() { Token = token };
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
    }
}
