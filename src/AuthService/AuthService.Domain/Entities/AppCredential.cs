using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AuthService.Domain.Entities
{
    [Table("IuCredentials")]
    public class AppCredential
    {
        public AppCredential(string name)
        {
            Name = name;
        }

        [Key]
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }
    }
}
