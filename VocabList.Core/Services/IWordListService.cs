using VocabList.Core.Entities;

namespace VocabList.Core.Services
{
    public interface IWordListService : IService<WordList>
    {
        Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId);
    }
}
