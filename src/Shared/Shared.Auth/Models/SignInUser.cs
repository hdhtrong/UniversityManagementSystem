using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.SharedAuth.Models
{
    public class SignInUser
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Code { get; set; }
        public string Picture { get; set; }
        public string Sub { get; set; }
        public string HostedDomain { get; set; }
        public string Audience { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Credentials { get; set; }
    }
}
