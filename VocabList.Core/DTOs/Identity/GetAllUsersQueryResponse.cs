namespace VocabList.Core.DTOs.Identity
{
    public class GetAllUsersQueryResponse
    {
        public List<CreateUserResponse> Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}
