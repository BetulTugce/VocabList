namespace VocabList.UI.Data.Users
{
    public class GetAllUsersQueryResponse
    {
        public List<User> Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}
