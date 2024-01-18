namespace VocabList.UserPortal.Data.Words
{
    public class GetAllWordsByUserIdAndWordListIdRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string AppUserId { get; set; }
        public int WordListId { get; set; }
    }
}
