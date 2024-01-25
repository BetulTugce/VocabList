namespace VocabList.Core.DTOs
{
    public class WordFilterResponse
    {
        public List<WordDto> Words { get; set; }
        public int TotalCount { get; set; }
    }
}
