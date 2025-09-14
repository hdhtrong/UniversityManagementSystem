using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class UserDto
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public string? Code { get; set; }

        public string? Type { get; set; }

        public string? Position { get; set; }

        public string? DepartmentCode { get; set; }

        public string? DepartmentName { get; set; }

        public List<string>? Roles { get; set; }
    }
}
