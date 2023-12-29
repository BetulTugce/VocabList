using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;
using VocabList.Service.Exceptions;


namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser model)
        {
            try
            {
                var createdUser = await _userService.CreateAsync(model);
                if (createdUser is not null)
                {
                    return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
                }
                return Created(string.Empty, createdUser);
            }
            catch (UserCreationException ex)
            {
                // UserCreationException'dan StatusCode'u alarak uygun HTTP durum kodunu döner.
                return StatusCode(int.Parse(ex.StatusCode), new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                // Genel hata durumu için 500 Internal Server Error döner.
                return StatusCode(500, new { ErrorMessage = "Kullanıcı kayıt işlemi başarısız oldu. " + ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get User By UserId", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { ErrorMessage = "Kullanıcı bulunamadı." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Kullanıcıyı getirme işlemi başarısız oldu. " + ex.Message });
            }
        }

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    try
        //    {
        //        return Ok(await _userService.GetAllUsersAsync());
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { ErrorMessage = "Kullanıcıları listeleme işlemi başarısız oldu. " + ex.Message });
        //    }
        //}

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest request)
        {
            try
            {
                List<CreateUserResponse> users = await _userService.GetAllUsersAsync(request.Page, request.Size);
                GetAllUsersQueryResponse response = new(){
                    TotalUsersCount = await _userService.GetTotalCountAsync(),
                    Users = users
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Kullanıcıları listeleme işlemi başarısız oldu. " + ex.Message });
            }
        }

        [HttpPost("update-password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword request)
        {
            if (request.Password.Equals(request.PasswordConfirm))
            {
                await _userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);
                return Ok();
            }
            else
            {
                throw new Exception($"Parolalar uyuşmuyor! Lütfen kontrol edin.");
            }
        }

        [HttpGet("get-roles-to-user/{userIdOrName}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To Users", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetRolesToUser(string userIdOrName)
        {
            string[] userRoles = await _userService.GetRolesToUserAsync(userIdOrName);
            return Ok(userRoles);
        }

        [HttpPost("assign-role-to-user")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Assign Role To User", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserRequest request)
        {
            try
            {
                await _userService.AssignRoleToUserAsnyc(request.UserId, request.Roles);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "İşlem başarısız! " + ex.Message });
            }
        }
    }
}
