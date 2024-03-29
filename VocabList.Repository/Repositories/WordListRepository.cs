﻿using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Drawing;
using System.Formats.Tar;
using VocabList.Core.DTOs;
using VocabList.Core.Entities;
using VocabList.Core.Entities.Identity;
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

        // Filtreleme seçeneklerine göre userin oluşturduğu kelime listelerini içeren bir liste ve toplam kelime listesi sayısını döndürür..
        public async Task<(List<WordList>, int)> GetFilteredWordListsAsync(WordListFilterRequest request)
        {
            #region Eski Filtreleme Kodları
            //// Kullanıcının idsi ile eşleşen kelime listelerini alır..
            //var query = _context.WordLists.AsNoTracking().Where(i => i.AppUserId == request.AppUserId);
            //IQueryable<WordList> wordListsQuery = null;

            //// Parametredeki değer null veya empty değilse..
            //if (!string.IsNullOrEmpty(request.Name))
            //    query = query.Where(w => w.Name.Contains(request.Name)); // Kelime listesinin adı parametredeki değeri içeriyor mu kontrol edilir..

            //// Sıralama yapılıyor..
            //if (request.OrderByDescending)
            //{
            //    // OrderByDescending true ise, tabloyu büyükten küçüğe (yani yeni tarihten eski tarihe) sıralar..
            //    query = query.OrderByDescending(i => i.CreatedDate);
            //}
            //else
            //{
            //    // OrderByDescending özelliği false ise, tabloyu küçükten büyüğe(yani eski tarihten yeni tarihe) sıralar..
            //    query = query.OrderBy(i => i.CreatedDate);
            //}

            //if (request.Page != -1 && request.Size != -1)
            //    wordListsQuery = query.Skip(request.Page * request.Size).Take(request.Size);
            //else
            //    wordListsQuery = query;

            //return (await wordListsQuery.ToListAsync(), query.Count());
            #endregion

            // Stored Procedure için gerekli parametreler oluşturuluyor.. 
            var parameters = new[]
            {
                // Arama metnini, eğer null değilse, SQL NULL olmayan bir değer olarak ayarlar.
                // Eğer null ise, parametre SQL NULL olacaktır.
                new SqlParameter("@SearchString", (object)request.SearchString ?? DBNull.Value),
                new SqlParameter("@Page", request.Page), // Sayfa numarası..
                new SqlParameter("@Size", request.Size), // Sayfa başına gösterilecek eleman sayısı..
                new SqlParameter("@Sort", request.Sort), // Sıralama yapılacak değişken..
                new SqlParameter("@OrderBy", request.OrderBy), //Sıralama şekli (ASC : küçükten büyüğe / a' dan z' ye / eskiden yeniye --- DESC : büyükten küçüğe / z' den a' ya / yeniden eskiye)
                new SqlParameter("@AppUserId", request.AppUserId),
                // Toplam eleman sayısını almak için bir çıkış parametresi ekleniyor..
                new SqlParameter
                {
                    ParameterName = "@TotalCount", // Parametre adı..
                    SqlDbType = SqlDbType.Int, // SQL tipi (int)..
                    Direction = ParameterDirection.Output // Çıkış parametresi olarak ayarlanıyor..
                }
            };

            // SQL sorgusunu çalıştırarak sonuçları alır ve bir liste olarak döndürür..
            var wordLists = await _context.WordLists.FromSqlRaw(
                "EXEC GetFilteredWordLists @SearchString, @Page, @Size, @Sort, @OrderBy, @AppUserId, @TotalCount OUTPUT", parameters)
                .ToListAsync();

            // Toplam eleman sayısını almak için OUTPUT parametresini kullanır.
            var totalCount = (int)parameters[6].Value; // @TotalCount'ın indexi 7

            return (wordLists, totalCount); // Kelimele listeleri ve toplam kelime listesi sayısını bir tuple olarak geriye döndürür.
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
