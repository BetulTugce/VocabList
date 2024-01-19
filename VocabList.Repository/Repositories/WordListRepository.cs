﻿using Azure;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class WordListRepository : GenericRepository<WordList>, IWordListRepository
    {
        public WordListRepository(VocabListDbContext context) : base(context)
        {
            
        }

        // İlgili idye sahip kullanıcının kelime listelerini page ve sizea göre liste şeklinde döndürür..
        public async Task<List<WordList>> GetAllWordListsByUserIdAsync(int page, int size, string userId)
        {
            if (page != -1 && size != -1)
            {
                return await _context.WordLists.AsNoTracking().Where(i => i.AppUserId == userId).Skip(page * size)
                  .Take(size).ToListAsync();
            }
            else
            {
                return await _context.WordLists.AsNoTracking().Where(i => i.AppUserId == userId).ToListAsync();
            }
            
        }

        // İlgili idye sahip kullanıcının toplam kaç tane kelime listesi varsa onu döndürür..
        public async Task<int> GetTotalCountByUserIdAsync(string userId)
        {
            return await _context.WordLists.Where(i => i.AppUserId == userId).CountAsync();
        }

        // Verilen id ve userIdye sahip WordListi arar. Eğer öğe bulunursa, bu öğe döndürülür; bulunamazsa null değeri döner.
        public async Task<WordList> GetWordListByIdAndUserIdAsync(int id, string userId)
        {
            return await _context.WordLists.FirstOrDefaultAsync(i => i.Id == id && i.AppUserId == userId);
        }
    }
}
