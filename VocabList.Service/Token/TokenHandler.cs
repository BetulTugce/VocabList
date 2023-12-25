using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VocabList.Core.Entities.Identity;
using VocabList.Repository.Token;

namespace VocabList.Service.Token
{
    public class TokenHandler : ITokenHandler
    {
        // IConfiguration, uygulamanın yapılandırma bilgilerine erişim sağlıyor.
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Token oluşturur..
        public Core.DTOs.Token CreateAccessToken(int second, AppUser appUser)
        {
            Core.DTOs.Token token = new();

            //Security Keyin simetriği alınıyor.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Security key verilen algoritmaya göre şifreleniyor. Yani şifrelenmiş kimlik oluşturuluyor.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Üretilecek token değerleri veriliyor.
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow, //O anki tarih bilgisini vererek token üretildiği an devreye girmesini sağlar.
                signingCredentials: signingCredentials, 
                claims: new List<Claim> { new(ClaimTypes.Name, appUser.UserName) }
                );

            // Token oluşturucu sınıfından bir örnek alınıyor.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);
            // RefreshToken oluşturuluyor.
            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            // Rastgele 32 bytelık sayı dizisi oluşturuluyor.
            byte[] number = new byte[32];
            // RandomNumberGenerator, IDisposable olduğu için using ile kullandım. Güvenli şekilde rastgele sayı üretebilmek için..
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            //  Random sayesinde number dizisine rastgele veri-byte yerleştiriyor.
            random.GetBytes(number);
            // Oluşturulan sayı dizisi Base64 formatına çevrilerek RefreshToken olarak döndürülüyor.
            return Convert.ToBase64String(number);
        }
    }
}
