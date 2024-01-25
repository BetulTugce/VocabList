using AutoMapper;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class WordService : Service<Word>, IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IMapper _mapper;

        public WordService(IGenericRepository<Word> repository, IUnitOfWork unitOfWork, IWordRepository wordRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _wordRepository = wordRepository;
            _mapper = mapper;
        }

        public async Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId)
        {
            return await _wordRepository.GetAllWordsByUserIdAndWordListIdAsync(page, size, wordListId, userId);
        }

        public async Task<(List<Word> Words, int TotalCount)> GetFilteredWordsAsync(string searchString, int page, int size, string sort, string orderBy, int wordListId, string appUserId)
        {
            return await _wordRepository.GetFilteredWordsAsync(searchString, page, size, sort, orderBy, wordListId, appUserId);
        }

        public async Task<int> GetTotalCountByWordListIdAsync(int wordListId)
        {
            return await _wordRepository.GetTotalCountByWordListIdAsync(wordListId);
        }
    }
}
