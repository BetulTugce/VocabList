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
    }
}
