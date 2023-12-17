using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class SentenceService : Service<Sentence>, ISentenceService
    {
        private readonly ISentenceRepository _sentenceRepository;

        public SentenceService(IGenericRepository<Sentence> repository, IUnitOfWork unitOfWork, ISentenceRepository sentenceRepository) : base(repository, unitOfWork)
        {
            _sentenceRepository = sentenceRepository;
        }
    }
}
