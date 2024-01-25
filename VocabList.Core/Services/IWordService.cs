using VocabList.Core.Entities;

namespace VocabList.Core.Services
{
    public interface IWordService : IService<Word>
    {
        Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId);
        Task<int> GetTotalCountByWordListIdAsync(int wordListId);

        Task<(List<Word> Words, int TotalCount)> GetFilteredWordsAsync(string searchString, int page, int size, string sort, string orderBy, int wordListId, string appUserId);
    }
}
