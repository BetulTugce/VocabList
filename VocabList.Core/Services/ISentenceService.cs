using VocabList.Core.Entities;

namespace VocabList.Core.Services
{
    public interface ISentenceService : IService<Sentence>
    {
        Task<List<Sentence>> GetAllByWordId(int WordId);
    }
}
