using System.ComponentModel.DataAnnotations;

namespace Jabbox.API.Models
{
    public class PostDTO
    {
        public string Message { get; set; }

        public string? PostedDate { get; set; }
    }
}
