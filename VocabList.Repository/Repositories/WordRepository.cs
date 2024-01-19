using Microsoft.EntityFrameworkCore;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class WordRepository : GenericRepository<Word>, IWordRepository
    {
        public WordRepository(VocabListDbContext context) : base (context)
        {
            
        }

        public async Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId)
        {
            if (page != -1 && size != -1)
            {
                return await _context.Words.AsNoTracking().Where(i => i.WordListId == wordListId && i.WordList.AppUserId == userId).Skip(page * size)
                  .Take(size).ToListAsync();
            }
            else
            {
                return await _context.Words.AsNoTracking().Where(i => i.WordListId == wordListId && i.WordList.AppUserId == userId).ToListAsync();
            }
        }

        public async Task<int> GetTotalCountByWordListIdAsync(int wordListId)
        {
            return await _context.Words.Where(i => i.WordListId == wordListId).CountAsync();
        }
    }
}
