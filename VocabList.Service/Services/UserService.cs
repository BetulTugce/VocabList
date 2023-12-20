using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Services;

namespace VocabList.Service.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            // Dependency Injection ile UserManager<AppUser> sınıfının bir örneğini bu servise enjekte eder.
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            // Yeni bir kullanıcı oluşturur.
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(), // Yeni bir GUID oluşturarak kullanıcıya bir ID atar.
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
            }, model.Password);

            // İşlem başarılıysa, oluşturulan kullanıcının bilgilerini alır ve CreateUserResponse tipine dönüştürür.
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                CreateUserResponse response = MapToUserDto(user);
                return response;
            }
            else
            {
                // İşlem başarısız olursa, IdentityResult nesnesindeki hata mesajlarını birleştirerek bir hata fırlatır.
                var errors = result.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errors);

                throw new Exception($"Kullanıcı kayıt işlemi başarısız oldu. Hata: {errorMessage}");
            }
        }

        public async Task<CreateUserResponse> GetUserByIdAsync(string userId)
        {
            // İlgili idye sahip kullanıcıyı bulur ve CreateUserResponse tipine dönüştürerek geriye döndürür.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            CreateUserResponse response = MapToUserDto(user);
            return response;
        }

        public async Task<List<CreateUserResponse>> GetAllUsersAsync()
        {
            // Tüm kullanıcıları getirir, hepsini MapToUserDto metodunu kullanarak CreateUserResponse tipine dönüştürür ve bir liste oluşturur.
            var users = await _userManager.Users.ToListAsync();
            return users.Select(user => MapToUserDto(user)).ToList();
        }

        private CreateUserResponse MapToUserDto(AppUser user)
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

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                // RefreshTokena güncel değeri atanıyor.
                user.RefreshToken = refreshToken;
                // RefreshTokenın expire olacağı zaman ayarlanıyor. 
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                // Veritabanında ilgili kolonlar güncelleniyor.
                await _userManager.UpdateAsync(user);
            }
            else
                throw new Exception("Kullanıcı veya şifre hatalı!");
        }

    }
}
