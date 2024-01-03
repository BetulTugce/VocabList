namespace VocabList.UserPortal.Data.Users
{
    public class CreateUserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        // 409 hatası için kullanılacak
        public bool IsConflictError { get; set; } = false;
    }
}
