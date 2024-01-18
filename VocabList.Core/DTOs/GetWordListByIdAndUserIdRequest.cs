namespace VocabList.Core.DTOs
{
    public class GetWordListByIdAndUserIdRequest
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
    }
}
