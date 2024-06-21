namespace Jabbox.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository Posts { get; }
        IAccountRepository Accounts { get; }

        void EnsureDatabaseCreated();
        void SeedData(string fileName);
        int SaveChanges();
    }
}