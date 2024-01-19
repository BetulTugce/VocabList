using VocabList.Core.Entities;

namespace VocabList.Core.Repositories
{
    public interface ISentenceRepository : IGenericRepository<Sentence>
    {
        Task<List<Sentence>> GetAllByWordId(int WordId);
    }
}
