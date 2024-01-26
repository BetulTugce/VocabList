using System.ComponentModel.DataAnnotations;

namespace VocabList.Core.DTOs
{
    public class WordListFilterRequest
    {
        //public string? Name { get; set; }
        public string? SearchString { get; set; }

        public string AppUserId { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }

        // Varsayılan olarak tabloyu küçükten-büyüğe/eskiden-yeniye (CreatedDatee göre) getirecek..
        //public bool OrderByDescending { get; set; } = false;

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        [StringLength(11)]
        public string Sort { get; set; } = "UpdatedDate";
        //public string Sort { get; set; } = "date";
        [StringLength(4)]
        public string OrderBy { get; set; } = "DESC";
    }
}
