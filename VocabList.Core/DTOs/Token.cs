namespace VocabList.Core.DTOs
{
    public class Token
    {
        public string AccessToken { get; set; } // Kullanıcının kimliğini doğrulamak ve yetkilendirmek için kullanılacak.
        public DateTime Expiration { get; set; } // AccessTokenın geçerlilik süresi.
        public string RefreshToken { get; set; } // AccessToken geçerliliğini kaybettiğinde kontrol edilecek, yeni bir token almak için kullanılacak.
    }
}
