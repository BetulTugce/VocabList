namespace VocabList.UserPortal.Data.WordLists
{
    public class GetAllWordListsByUserIdRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string AppUserId { get; set; }
    }
}
