namespace VocabList.Core.DTOs.Identity
{
    public class LoginUserResponse
    {
        public Token Token { get; set; }
        public CreateUserResponse User { get; set; }
    }
}
