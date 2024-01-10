using System.Linq.Expressions;

namespace VocabList.Core.Repositories
{
    //Bu interface, ORM (Object-Relational Mapping) aracı ile kullanılacak genel CRUD (Create, Read, Update, Delete) işlemlerini içeriyor..
    public interface IGenericRepository<T> where T : class
    {
        #region Read Methodları

        // Belirli bir nesnenin, kimliği (IDsi) üzerinden asenkron olarak getirilmesini sağlar.
        Task<T> GetByIdAsync(int id);

        // Tüm nesnelerin getirilmesini sağlar.
        IQueryable<T> GetAll();

        // Belirli bir koşula uyan nesneleri getirir.
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        // Belirli bir koşula uyan nesne var mı yok mu kontrolünü asenkron olarak yapar.
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        // Belirtilen koşulu sağlayan ilk öğeyi asenkron olarak getirir.
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        #endregion


        #region Write Methodları

        // Belirli bir nesnenin eklenmesini asenkron olarak sağlar.
        Task AddAsync(T entity);

        // Birden çok nesnenin eklenmesini asenkron olarak sağlar.
        Task AddRangeAsync(IEnumerable<T> entities);

        // Bir nesnenin güncellenmesini sağlar.
        bool Update(T entity);

        // Belirli bir nesnenin silinmesini sağlar.
        void Remove(T entity);

        // Birden çok nesnenin silinmesini sağlar.
        void RemoveRange(IEnumerable<T> entities);

        // Tüm nesnelerin sayısını asenkron olarak getirir.
        Task<int> GetTotalCountAsync();

        #endregion
    }
}
