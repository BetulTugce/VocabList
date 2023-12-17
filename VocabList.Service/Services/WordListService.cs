using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class WordListService : Service<WordList>, IWordListService
    {
        private readonly IWordListRepository _wordListRepository;

        public WordListService(IGenericRepository<WordList> repository, IUnitOfWork unitOfWork, IWordListRepository wordListRepository) : base(repository, unitOfWork)
        {
            _wordListRepository = wordListRepository;
        }
    }
}
