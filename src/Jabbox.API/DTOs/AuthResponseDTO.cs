namespace Jabbox.API.Models
{
    public class AuthResponseDTO
    {
        public string UserName { get; set; }
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
