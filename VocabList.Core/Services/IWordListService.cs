using VocabList.Core.Entities;

namespace VocabList.Core.Services
{
    public interface IWordListService : IService<WordList>
    {
        Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId);
        Task<int> GetTotalCountByUserIdAsync(string userId);

        Task<WordList> GetWordListByIdAndUserIdAsync(int id, string userId);
    }
}
