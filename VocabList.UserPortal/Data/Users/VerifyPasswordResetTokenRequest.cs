namespace VocabList.UserPortal.Data.Users
{
    public class VerifyPasswordResetTokenRequest
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
    }
}
