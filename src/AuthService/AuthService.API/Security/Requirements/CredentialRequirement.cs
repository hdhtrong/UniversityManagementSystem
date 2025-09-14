using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.API.Security.Requirements
{
    public class CredentialRequirement : IAuthorizationRequirement
    {
        public string ClaimValue { get; }

        public CredentialRequirement( string claimValue)
        {
            ClaimValue = claimValue;
        }
    }
}
