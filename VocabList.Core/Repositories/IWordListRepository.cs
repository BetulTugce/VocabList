using VocabList.Core.DTOs;
using VocabList.Core.Entities;

namespace VocabList.Core.Repositories
{
    public interface IWordListRepository : IGenericRepository<WordList>
    {
        Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId);
        Task<int> GetTotalCountByUserIdAsync(string userId);

        Task<WordList> GetWordListByIdAndUserIdAsync(int id, string userId);

        // Filtreleme seçeneklerine göre userin oluşturduğu kelime listelerini içeren bir liste ve toplam kelime listesi sayısını döndürür..
        Task<(List<WordList>, int)> GetFilteredWordListsAsync(WordListFilterRequest request);
    }
}
