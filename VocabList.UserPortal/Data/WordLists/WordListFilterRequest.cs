namespace VocabList.UserPortal.Data.WordLists
{
    public class WordListFilterRequest
    {
        public string? Name { get; set; }

        public string AppUserId { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }

        // Varsayılan olarak tabloyu küçükten-büyüğe/eskiden-yeniye (CreatedDatee göre) getirecek..
        public bool OrderByDescending { get; set; } = false;

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
