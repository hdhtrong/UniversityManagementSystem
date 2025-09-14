using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.API.Security.Requirements;

namespace AuthService.API.Security
{
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder AddCredentialRequirement(this AuthorizationPolicyBuilder builder, params string[] allowedValues)
        {
            foreach (var claimValue in allowedValues)
            {
                builder.AddRequirements(new CredentialRequirement(claimValue));
            }
            return builder;
        }
    }
}
