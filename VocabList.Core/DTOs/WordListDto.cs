using VocabList.Core.DTOs.Common;

namespace VocabList.Core.DTOs
{
    public class WordListDto : BaseDto
    {
        public string Name { get; set; }

        public string AppUserId { get; set; }
    }
}
