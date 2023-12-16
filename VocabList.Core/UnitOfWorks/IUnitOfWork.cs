namespace VocabList.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        // Asenkron olarak değişiklikleri kaydeder.
        Task CommitAsync();

        // Senkron olarak değişiklikleri kaydeder.
        void Commit();
    }
}
