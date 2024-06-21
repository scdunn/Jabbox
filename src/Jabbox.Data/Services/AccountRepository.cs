using Jabbox.Data.Interfaces;
using Jabbox.Data.Models;
using System.Security.Cryptography;
using System.Text;

namespace Jabbox.Data.Services
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private (string, string) HashPasword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        }

        private bool VerifyPassword(string password, string hash, string salt)
        {
            var saltBytes = Enumerable.Range(0, salt.Length)
                      .Where(x => x % 2 == 0)
                      .Select(x => Convert.ToByte(salt.Substring(x, 2), 16))
                      .ToArray();
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        public AccountRepository(DataContext context) : base(context) { }
        public Account Login(string userName, string password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Username == userName);
            if (account != null && VerifyPassword(password, account.PasswordHash, account.PasswordSalt))
            {
                return account;
            }

            return null;

        }
        public Account Register(string userName, string password)
        {
            var account = new Account();
            account.Username = userName;

            (string hashed, string salt) = HashPasword(password);

            account.PasswordHash = hashed;
            account.PasswordSalt = salt;
            _context.Accounts.Add(account);
            return account;
        }

        
    }
}