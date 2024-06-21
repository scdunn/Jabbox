namespace Jabbox.Data.Models
{ 
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public ICollection<Post> Posts { get; set; }

    }

}