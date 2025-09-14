using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole Role { get; set; }
    }

}
