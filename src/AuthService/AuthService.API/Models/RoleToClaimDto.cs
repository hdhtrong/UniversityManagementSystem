namespace AuthService.API.Models
{
    public class RoleToClaimDto
    {
        public required string RoleName { get; set; }
        public required string ClaimValue { get; set; }
    }
}
