using System.ComponentModel.DataAnnotations;

namespace VocabList.UserPortal.Data.Words
{
    public class WordFilterRequest
    {
        public string? SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        [StringLength(11)]
        public string Sort { get; set; } = "UpdatedDate";
        //public string Sort { get; set; } = "date";
        [StringLength(4)]
        public string OrderBy { get; set; } = "DESC";

        public int WordListId { get; set; }
        public string AppUserId { get; set; }
    }
}
