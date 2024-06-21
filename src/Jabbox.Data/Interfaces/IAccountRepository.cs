using Jabbox.Data.Models;

namespace Jabbox.Data.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Account Login(string userName, string password);
        Account Register(string userName, string password);
    }
}