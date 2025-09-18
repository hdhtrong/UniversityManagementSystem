namespace AuthService.API.Models
{
    public class ResetPasswordByLinkDto
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
