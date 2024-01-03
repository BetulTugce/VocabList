namespace VocabList.Core.DTOs.Identity
{
    public class PasswordResetRequest
    {
        public string Email { get; set; }
        public bool IsUserPortal { get; set; }
    }
}
