using System.ComponentModel.DataAnnotations;

namespace Jabbox.API.Models
{
    public class AuthRegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
