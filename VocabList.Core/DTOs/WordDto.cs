using VocabList.Core.DTOs.Common;

namespace VocabList.Core.DTOs
{
    public class WordDto : BaseDto
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public int? WordListId { get; set; }
    }
}
