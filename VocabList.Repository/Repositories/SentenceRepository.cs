using Microsoft.EntityFrameworkCore;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class SentenceRepository : GenericRepository<Sentence>, ISentenceRepository
    {
        public SentenceRepository(VocabListDbContext context) : base(context)
        {
            
        }

        public async Task<List<Sentence>> GetAllByWordId(int WordId)
        {
            return await _context.Sentences.Where(i => i.WordId == WordId).ToListAsync();
        }
    }
}
