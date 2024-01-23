namespace VocabList.Core.DTOs
{
    public class WordListFilterResponse
    {
        public List<WordListDto> WordLists { get; set; }
        public int TotalCount { get; set; }
    }
}
