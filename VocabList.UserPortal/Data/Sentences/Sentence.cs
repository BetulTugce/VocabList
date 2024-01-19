namespace VocabList.UserPortal.Data.Sentences
{
    public class Sentence
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string Value { get; set; }

        public int WordId { get; set; }
    }
}
