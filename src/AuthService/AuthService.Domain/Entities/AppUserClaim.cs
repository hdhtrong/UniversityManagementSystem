using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser User { get; set; }
    }
}
