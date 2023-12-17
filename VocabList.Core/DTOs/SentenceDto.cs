using VocabList.Core.DTOs.Common;

namespace VocabList.Core.DTOs
{
    public class SentenceDto : BaseDto
    {
        public string Value { get; set; }

        public int? WordId { get; set; }
    }
}
