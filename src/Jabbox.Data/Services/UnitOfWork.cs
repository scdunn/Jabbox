using Jabbox.Data.Interfaces;
using System.Xml.Linq;

namespace Jabbox.Data.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IAccountRepository Accounts { get; private set; }
        public IPostRepository Posts { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;

            // create repositories
            Posts = new PostRepository(_context);
            Accounts = new AccountRepository(_context);
        }

        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureCreated();
        }

        public void SeedData(string filePath)
        {
            Random r = new Random();
            XElement seedData = XElement.Load(filePath);

            foreach(var xAccount in seedData.Elements())
            {
                var userName = (string)xAccount.Element("username").Value;
                var password = (string)xAccount.Element("password").Value;

                var account = Accounts.Register(userName, password);

                DateTime postedDate = DateTime.Now;

                foreach (var xPost in xAccount.Descendants("post"))
                {
                    var message = (string)xPost.Element("message");
                    postedDate = DateTime.Now.AddHours(-r.Next(1, 24));
                                           
                    var post = Posts.CreatePost(account, message, postedDate);
                }
            }

            SaveChanges();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public void Dispose() => _context.Dispose();
   
    }
}