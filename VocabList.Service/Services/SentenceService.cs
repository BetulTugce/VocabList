using AutoMapper;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class SentenceService : Service<Sentence>, ISentenceService
    {
        private readonly ISentenceRepository _sentenceRepository;
        private readonly IMapper _mapper;

        public SentenceService(IGenericRepository<Sentence> repository, IUnitOfWork unitOfWork, ISentenceRepository sentenceRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _sentenceRepository = sentenceRepository;
            _mapper = mapper;
        }
    }
}
