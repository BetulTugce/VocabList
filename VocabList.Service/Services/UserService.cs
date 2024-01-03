using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Entities;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Service.Exceptions;
using VocabList.Service.Helpers;

namespace VocabList.Service.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        readonly IEndpointRepository _endpointRepository;

        //public int TotalUsersCount => _userManager.Users.Count();

        public UserService(UserManager<AppUser> userManager, IEndpointRepository endpointRepository)
        {
            // Dependency Injection ile UserManager<AppUser> sınıfının bir örneğini bu servise enjekte eder.
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _endpointRepository = endpointRepository;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            // E-posta adresinin benzersiz olup olmadığını kontrol eder..
            var existingEmailUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                // Eğer e-posta adresi başka bir kullanıcı tarafından kullanılıyorsa hata döndürür..
                //throw new UserCreationException("Bu e-posta adresi zaten kullanımda.", 409.ToString());

                return new()
                {

                };
            }
            else
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

                    // Bir kullanıcı oluşturulduğunda varsayılan olarak üye rolünü alacak.. Gerektiğinde kişiye diğer rol atamalarını yönetim panelindeki yetkili kullanıcı yapacak..
                    await _userManager.AddToRoleAsync(user, "Member");

                    return response;
                }
                else
                {
                    // İşlem başarısız olursa, IdentityResult nesnesindeki hata kodlarına özel durumları ele alır.

                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            // Bu kullanıcı adı zaten kullanımda ise 409 Conflict durum kodu ile birlikte hata döner.
                            throw new UserCreationException("Bu kullanıcı adı zaten kullanımda.", 409.ToString());
                        }
                    }

                    // İşlem başarısız olursa, IdentityResult nesnesindeki hata mesajlarını birleştirerek bir hata fırlatır.
                    var errors = result.Errors.Select(error => error.Description);
                    var errorMessage = string.Join(", ", errors);

                    throw new Exception($"Kullanıcı kayıt işlemi başarısız oldu. Hata: {errorMessage}");
                }
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

        //public async Task<List<CreateUserResponse>> GetAllUsersAsync()
        //{
        //    // Tüm kullanıcıları getirir, hepsini MapToUserDto metodunu kullanarak CreateUserResponse tipine dönüştürür ve bir liste oluşturur.
        //    var users = await _userManager.Users.ToListAsync();
        //    return users.Select(user => MapToUserDto(user)).ToList();
        //}

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

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Kullanıcı bulunduysa resetToken decode ediliyor..
                resetToken = resetToken.UrlDecode();
                // Parola güncellemesi yapılıyor..
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    // Kullanıcının kimlik bilgilerindeki değişiklik sisteme yansıtılır, bu sayede resetToken çürütülür yani parola değiştirmek için aynı resetToken kullanılamaz.
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new Exception($"Parola değişikliği sırasında bir sorun oluştu, işlem başarısız!");
            }
        }

        public async Task<List<CreateUserResponse>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users.OrderBy(i => i.UserName)
                  .Skip(page * size)
                  .Take(size)
                  .ToListAsync();
            // MapToUserDto metodunu kullanarak CreateUserResponse tipine dönüştürür ve bir liste oluşturur.
            return users.OrderBy(i => i.UserName).Select(user => MapToUserDto(user)).ToList();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            AppUser user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            // Kullanıcının nameine karşılık tüm rolleri çekilir..
            var userRoles = await GetRolesToUserAsync(name);

            // Kullanıcının rolü yoksa yetkisi yok demektir..
            if (!userRoles.Any())
                return false;

            // Codeu parametrede gelen codea karşılık olan endpoint rollerle beraber elde edilir..
            Endpoint? endpoint = await _endpointRepository.GetAll()
                     .Include(e => e.Roles)
                     .FirstOrDefaultAsync(e => e.Code == code);

            // Endpoint nullsa yetki yok..
            if (endpoint == null)
                return false;

            var hasRole = false;

            // Endpointe atanan rollerin sadece isimleri alınıyor..
            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            //foreach (var userRole in userRoles)
            //{
            //    if (!hasRole)
            //    {
            //        foreach (var endpointRole in endpointRoles)
            //            if (userRole == endpointRole)
            //            {
            //                hasRole = true;
            //                break;
            //            }
            //    }
            //    else
            //        break;
            //}

            //return hasRole;

            // Eğer endpointe atanan rollerden birisi ile kullanıcının sahip olduğu rol ya da rollerden biri uyuşuyorsa yetkisi var demektir..
            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            // Yetki yok..
            return false;
        }

        public async Task<CreateUserResponse> GetUserByNameorEmailAsync(string userNameorEmail)
        {
            // İlgili idye sahip kullanıcıyı bulur ve CreateUserResponse tipine dönüştürerek geriye döndürür.
            var user = await _userManager.FindByEmailAsync(userNameorEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userNameorEmail);
                if (user == null)
                {
                    return null;
                }

            }
            CreateUserResponse response = MapToUserDto(user);
            return response;
        }
    }
}
