﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.DTOs;
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

        public async Task<(List<WordList>, int)> GetFilteredWordListsAsync(WordListFilterRequest request)
        {
            return await _wordListRepository.GetFilteredWordListsAsync(request);
        }

        public async Task<int> GetTotalCountByUserIdAsync(string userId)
        {
            return await _wordListRepository.GetTotalCountByUserIdAsync(userId);
        }

        public async Task<WordList> GetWordListByIdAndUserIdAsync(int id, string userId)
        {
            return await _wordListRepository.GetWordListByIdAndUserIdAsync(id, userId);
        }
    }
}
