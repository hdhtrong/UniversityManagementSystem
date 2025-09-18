namespace AuthService.API.Models
{
    public class UserToClaimDto
    {
        public required string Email { get; set; }
        public required string ClaimValue { get; set; }
    }
}
