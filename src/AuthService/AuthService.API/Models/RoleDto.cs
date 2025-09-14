using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class RoleDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
