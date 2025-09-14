using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Models
{
    public class CredentialDto
    {
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }
    }
}
