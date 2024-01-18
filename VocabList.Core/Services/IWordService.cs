using VocabList.Core.Entities;

namespace VocabList.Core.Services
{
    public interface IWordService : IService<Word>
    {
        Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId);
    }
}
