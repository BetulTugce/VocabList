namespace VocabList.Core.DTOs
{
    public class GetAllWordListsByUserIdRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string AppUserId { get; set; }
    }
}
