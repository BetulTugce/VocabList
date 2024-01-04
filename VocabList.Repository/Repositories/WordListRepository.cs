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

        public async Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId)
        {
            return await _context.WordLists.AsNoTracking().Where(i => i.AppUserId == userId).Skip(page * size)
                  .Take(size).ToListAsync();
        }
    }
}
