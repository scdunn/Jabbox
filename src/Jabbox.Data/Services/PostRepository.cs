using Jabbox.Data.Interfaces;
using Jabbox.Data.Models;

namespace Jabbox.Data.Services
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context) { }
        public IQueryable<Post> GetPosts(string  userName)
        {
            return _context.Posts.Where(p => p.Account.Username == userName).OrderByDescending(p=>p.PostedDate);
        }

        public Post AddPost(string userName, string message)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Username == userName);
            return CreatePost(account, message, DateTime.Now);
 
        }

        public Post CreatePost(Account account, string message, DateTime postedDate)
        {
            var newPost = new Post();
            newPost.Message = message;

            newPost.PostedDate = postedDate;
            newPost.Account = account;
            _context.Posts.Add(newPost);
            return newPost;
        }
    }
}