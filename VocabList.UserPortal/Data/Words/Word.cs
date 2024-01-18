namespace VocabList.UserPortal.Data.Words
{
    public class Word
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public int WordListId { get; set; }
    }
}
