using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;
using VocabList.Service.Exceptions;
using VocabList.Service.Services;


namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly FileService _fileService;

        public UsersController(IUserService userService, FileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
        }

        [HttpPost("fileupload")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Upload Profile Image by UserId", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Dosya boyutu kontrol ediliyor..
            if (file.Length > 10 * 1024 * 1024) // 10MB sınırı
            {
                return BadRequest("Dosya boyutu 10MB'tan büyük olamaz.");
            }
            // Dosya adı rastgele oluşturuluyor..
            string randomFileName = _fileService.GenerateRandomFileName(file.FileName);

            // Dosya yolu alınıyor..
            string filePath = _fileService.GetFilePath(randomFileName);

            // Dosya kaydediliyor..
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(new { FileName = randomFileName, FilePath = filePath });
        }

        [HttpPut("update-profile-image")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Updating, Definition = "Update Profile Image by UserId")]
        public async Task<IActionResult> UpdateProfileImage(UpdateProfileImageRequest request)
        {
            try
            {
                // Kullanıcının idsi ile dosya adı (nullable) parametre ile gönderilerek ProfileImagePath kolonu güncelleniyor..
                var response = await _userService.UpdateUserProfileImageAsync(request.UserId, request.Path);

                // İşlem başarılı değilse 400 döndürür..
                if (!response.Item1)
                {
                    return BadRequest();
                }

                // Kullanıcının önceden yüklediği bir resim varsa siliniyor..
                if (response.Item2 != null)
                {
                    _fileService.DeleteProfileImage(response.Item2);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Beklenmeyen bir hata meydana geldi." + ex.Message });
            }
        }

        [HttpGet("profile-image/{userId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Profile Image by UserId")]
        public async Task<IActionResult> GetProfileImage(string userId)
        {
            // Yapılan istek neticesinde ProfileImagePath ve user bilgisi tuple olarak döner..
            var response = await _userService.GetPofileImageByUserIdAsync(userId);

            if (response == null)
            {
                return NotFound();
            }

            //if (response.Item2 == null)
            //{ // Tupledaki 2.item yani user bilgisi null gelirse kullanıcı bulunamadı hatası verir..
            //    return NotFound(new { ErrorMessage = "Kullanıcı bulunamadı." });
            //}

            //if (string.IsNullOrEmpty(response.Item1))
            //{ // Tupledaki 1.item yani ProfileImagePath null gelirse resim bulunamadı hatası verir..
            //    return NotFound("Profil resmi bulunamadı.");
            //}

            //byte[] imageBytes = await _fileService.GetProfileImageAsync(response.Item1);
            byte[] imageBytes = await _fileService.GetProfileImageAsync(response);

            // Dosya uzantısına göre formatı belirlemek için..
            // string fileExtension = Path.GetExtension(response.Item1);
            // return File(imageBytes, $"image/{fileExtension.Replace(".", "")}");

            return File(imageBytes, "image/jpeg"); // Resmi JPEG olarak döndürüyor..
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

        [HttpGet("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Total Count")]
        public async Task<IActionResult> GetTotalCount()
        {
            //Toplam kullanıcı sayısını döner..
            return Ok(await _userService.GetTotalCountAsync());
        }
    }
}
