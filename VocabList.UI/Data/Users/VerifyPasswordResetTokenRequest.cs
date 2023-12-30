namespace VocabList.UI.Data.Users
{
    public class VerifyPasswordResetTokenRequest
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
    }
}
