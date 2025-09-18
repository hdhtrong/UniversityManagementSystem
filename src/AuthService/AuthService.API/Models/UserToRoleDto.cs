namespace AuthService.API.Models
{
    public class UserToRoleDto
    {
        public required string Email { get; set; }
        public required string RoleName { get; set; }
    }
}
