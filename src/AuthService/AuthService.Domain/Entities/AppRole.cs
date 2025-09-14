using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole() : base()
        {
        } 

        public AppRole(string name) : base(name)
        {
        }

        public AppRole(string name, string description) : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }
    }
}
