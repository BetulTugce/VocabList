using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class WordRepository : GenericRepository<Word>, IWordRepository
    {
        public WordRepository(VocabListDbContext context) : base (context)
        {
            
        }

        public async Task<List<Word>> GetAllWordsByUserIdAndWordListIdAsync(int page, int size, int wordListId, string userId)
        {
            if (page != -1 && size != -1)
            {
                return await _context.Words.AsNoTracking().Where(i => i.WordListId == wordListId && i.WordList.AppUserId == userId).Skip(page * size)
                  .Take(size).ToListAsync();
            }
            else
            {
                return await _context.Words.AsNoTracking().Where(i => i.WordListId == wordListId && i.WordList.AppUserId == userId).ToListAsync();
            }
        }

        // Filtreleme koşuluna uyan kelimeleri ve bu koşulu sağlayan toplam kelime sayısını getirir.. Veritabanında yazılan GetFilteredWords isimli Stored Procedureü kullanıyor.. 
        public async Task<(List<Word> Words, int TotalCount)> GetFilteredWordsAsync(string searchString, int page, int size, string sort, string orderBy, int wordListId, string appUserId)
        {
            #region AppUserId ve WordListId kontrolü olmadan
            //var parameters = new[]
            //{
            //    new SqlParameter("@SearchString", (object)searchString ?? DBNull.Value),
            //    new SqlParameter("@Page", page),
            //    new SqlParameter("@Size", size),
            //    new SqlParameter("@Sort", sort),
            //    new SqlParameter("@OrderBy", orderBy),
            //    new SqlParameter
            //    {
            //        ParameterName = "@TotalCount",
            //        SqlDbType = SqlDbType.Int,
            //        Direction = ParameterDirection.Output
            //    }
            //};

            //var words = await _context.Words.FromSqlRaw(
            //    "EXEC GetFilteredWords @SearchString, @Page, @Size, @Sort, @OrderBy, @TotalCount OUTPUT", parameters)
            //    .ToListAsync();

            //var totalCount = (int)parameters[5].Value;

            //return (words, totalCount);
            #endregion

            // Stored Procedure için gerekli parametreler oluşturuluyor.. 
            var parameters = new[]
            {
                // Arama metnini, eğer null değilse, SQL NULL olmayan bir değer olarak ayarlar.
                // Eğer null ise, parametre SQL NULL olacaktır.
                new SqlParameter("@SearchString", (object)searchString ?? DBNull.Value),
                new SqlParameter("@Page", page), // Sayfa numarası..
                new SqlParameter("@Size", size), // Sayfa başına gösterilecek eleman sayısı..
                new SqlParameter("@Sort", sort), // Sıralama yapılacak değişken..
                new SqlParameter("@OrderBy", orderBy), //Sıralama şekli (ASC : küçükten büyüğe / a' dan z' ye / eskiden yeniye --- DESC : büyükten küçüğe / z' den a' ya / yeniden eskiye)
                new SqlParameter("@WordListId", wordListId),
                new SqlParameter("@AppUserId", appUserId),
                // Toplam eleman sayısını almak için bir çıkış parametresi ekleniyor..
                new SqlParameter
                {
                    ParameterName = "@TotalCount", // Parametre adı..
                    SqlDbType = SqlDbType.Int, // SQL tipi (int)..
                    Direction = ParameterDirection.Output // Çıkış parametresi olarak ayarlanıyor..
                }
            };

            // SQL sorgusunu çalıştırarak sonuçları alır ve bir liste olarak döndürür..
            var words = await _context.Words.FromSqlRaw(
                "EXEC GetFilteredWords @SearchString, @Page, @Size, @Sort, @OrderBy, @WordListId, @AppUserId, @TotalCount OUTPUT", parameters)
                .ToListAsync();

            // Toplam eleman sayısını almak için OUTPUT parametresini kullanır.
            var totalCount = (int)parameters[7].Value; // @TotalCount'ın indexi 7

            return (words, totalCount); // Kelimeleri ve toplam kelime sayısını bir tuple olarak geriye döndürür.
        }

        public async Task<int> GetTotalCountByWordListIdAsync(int wordListId)
        {
            return await _context.Words.Where(i => i.WordListId == wordListId).CountAsync();
        }
    }
}
