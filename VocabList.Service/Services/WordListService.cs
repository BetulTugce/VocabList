using AutoMapper;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class WordListService : Service<WordList>, IWordListService
    {
        private readonly IWordListRepository _wordListRepository;
        private readonly IMapper _mapper;

        public WordListService(IGenericRepository<WordList> repository, IUnitOfWork unitOfWork, IWordListRepository wordListRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _wordListRepository = wordListRepository;
            _mapper = mapper;
        }

        public async Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId)
        {
            return await _wordListRepository.GetAllWordListsByUserIdAsync(page, size, userId);
        }
    }
}
