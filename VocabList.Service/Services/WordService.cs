using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class WordService : Service<Word>, IWordService
    {
        private readonly IWordRepository _wordRepository;

        public WordService(IGenericRepository<Word> repository, IUnitOfWork unitOfWork, IWordRepository wordRepository) : base(repository, unitOfWork)
        {
            _wordRepository = wordRepository;
        }
    }
}
