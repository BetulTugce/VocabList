using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace VocabList.Service.Helpers
{
    static public class CustomEncoders
    {
        public static string UrlEncode(this string value)
        {
            // Gelen value bytea dönüştürülüp diziye atanıyor..
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            // Value urlde taşınabilir bir stringe dönüştürerek şifreliyor..
            return WebEncoders.Base64UrlEncode(bytes);
        }

        public static string UrlDecode(this string value)
        {
            byte[] tokenBytes = WebEncoders.Base64UrlDecode(value);
            return Encoding.UTF8.GetString(tokenBytes);
        }
    }
}
