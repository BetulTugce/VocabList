using Azure;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class WordListRepository : GenericRepository<WordList>, IWordListRepository
    {
        public WordListRepository(VocabListDbContext context) : base(context)
        {
            
        }

        // İlgili idye sahip kullanıcının kelime listelerini page ve sizea göre liste şeklinde döndürür..
        public async Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId)
        {
            return await _context.WordLists.AsNoTracking().Where(i => i.AppUserId == userId).Skip(page * size)
                  .Take(size).ToListAsync();
        }

        // İlgili idye sahip kullanıcının toplam kaç tane kelime listesi varsa onu döndürür..
        public async Task<int> GetTotalCountByUserIdAsync(string userId)
        {
            return await _context.WordLists.Where(i => i.AppUserId == userId).CountAsync();
        }
    }
}
