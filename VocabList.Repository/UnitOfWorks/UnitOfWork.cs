using VocabList.Core.UnitOfWorks;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VocabListDbContext _context;

        public UnitOfWork(VocabListDbContext context)
        {
            _context = context;
        }

        // Senkron olarak değişiklikleri kaydetmeyi sağlayan metot.
        public void Commit()
        {
            // SaveChanges metodu, veritabanındaki tüm değişiklikleri kalıcı hale getirir.
            _context.SaveChanges();
        }

        // Asenkron olarak değişiklikleri kaydetmeyi sağlayan metot.
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
