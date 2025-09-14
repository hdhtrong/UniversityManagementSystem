using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class AppUser : IdentityUser<string>
    {
        [MaxLength(150)]
        public string DepartmentCode{ get; set; }

        [MaxLength(150)]
        public string DepartmentName { get; set; }

        [MaxLength(150)]
        public string Position { get; set; }        // chuyên viên, trường phòng, v.v
        
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(20)]
        public string Type { get; set; } // Sinh viên/CBVC

        [MaxLength(250)]
        public string DisplayName { get; set; }

        public virtual ICollection<AppUserClaim> Claims { get; set; }
        public virtual ICollection<AppUserLogin> Logins { get; set; }
        public virtual ICollection<AppUserToken> Tokens { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
