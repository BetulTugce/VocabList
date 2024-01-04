namespace VocabList.UserPortal.Data.WordLists
{
    public class WordList
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string Name { get; set; }

        public string AppUserId { get; set; }
    }
}
