using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;


namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname
            }, model.Password);
            if (result.Succeeded)
            {
                //return StatusCode((int)HttpStatusCode.Created);

                var createdUser = await _userManager.FindByNameAsync(model.Username);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            else
            {
                //return BadRequest(new { ErrorMessage = "Kullanıcı oluşturma başarısız oldu", Errors = result.Errors });
                var errors = result.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errors);

                return BadRequest(new { ErrorMessage = "Kullanıcı kayıt işlemi başarısız oldu.", Errors = errorMessage });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { ErrorMessage = "Kullanıcı bulunamadı." });
            }

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

    }
}
