using VocabList.Core.Entities;

namespace VocabList.Core.Repositories
{
    public interface IWordRepository : IGenericRepository<Word>
    {
        Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId);
    }
}
