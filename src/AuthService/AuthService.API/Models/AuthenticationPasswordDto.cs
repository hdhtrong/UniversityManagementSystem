namespace AuthService.API.Models
{
    public class AuthenticationPasswordDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
