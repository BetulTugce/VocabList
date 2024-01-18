namespace VocabList.UserPortal.Data.Words
{
    public class CreateWordRequest
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public int WordListId { get; set; }
    }
}
