using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly VocabListDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // Bu constructor, GenericRepository sınıfını başlatır ve bir DbContext örneği alır.
        public GenericRepository(VocabListDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Veritabanına asenkron olarak bir nesne ekler.
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Veritabanına asenkron olarak bir nesne koleksiyonu ekler.
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Belirli bir koşula uyan nesne var mı yok mu kontrolünü asenkron olarak yapar.
        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        // Tüm nesneleri getirir ve bu nesnelerin izlenmesini devre dışı bırakır.
        public IQueryable<T> GetAll()
        {
            //return _dbSet.AsNoTracking().AsQueryable();
            return _dbSet.AsQueryable();
        }

        // Belirli bir ID'ye sahip nesneyi asenkron olarak getirir.
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
                query = _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        // Tüm nesneleri alır ve asenkron olarak sayısını getirir.
        public Task<int> GetTotalCountAsync()
        {
            return _dbSet.CountAsync();
        }

        // Bir nesneyi veritabanından kaldırır.
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Bir nesne koleksiyonunu veritabanından kaldırır.
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // Bir nesneyi günceller.
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Belirli bir koşula uyan nesneleri getirir.
        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
