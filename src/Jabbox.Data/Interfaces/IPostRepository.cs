using Jabbox.Data.Models;

namespace Jabbox.Data.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        IQueryable<Post> GetPosts(string userName);
        Post AddPost(string userName, string message);
        Post CreatePost(Account account, string message, DateTime postedDate);
    }
}