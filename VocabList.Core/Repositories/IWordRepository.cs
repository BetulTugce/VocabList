using VocabList.Core.Entities;

namespace VocabList.Core.Repositories
{
    public interface IWordRepository : IGenericRepository<Word>
    {
        Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId);
        Task<int> GetTotalCountByWordListIdAsync(int wordListId);

        // Filtreleme koşuluna uyan kelimeleri ve bu koşulu sağlayan toplam kelime sayısını getirir.. Veritabanında yazılan GetFilteredWords isimli Stored Procedureü kullanıyor..
        Task<(List<Word> Words, int TotalCount)> GetFilteredWordsAsync(string searchString, int page, int size, string sort, string orderBy, int wordListId, string appUserId);
    }
}
