using System.ComponentModel.DataAnnotations;

namespace VocabList.Core.DTOs
{
    public class WordFilterRequest
    {
        public string? SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        [StringLength(11)]
        public string Sort { get; set; } = "CreatedDate";
        //public string Sort { get; set; } = "date";
        [StringLength(4)]
        public string OrderBy { get; set; } = "desc";

        public int WordListId { get; set; }
        public string AppUserId { get; set; }
    }
}
