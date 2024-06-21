namespace Jabbox.Data.Models
{ 
    public class Post
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public Account? Account { get; set; }

    }

}