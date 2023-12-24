namespace VocabList.Core.DTOs.Identity
{
    public class AssignRoleToUserRequest
    {
        public string UserId { get; set; }
        public string[] Roles { get; set; }
    }
}
