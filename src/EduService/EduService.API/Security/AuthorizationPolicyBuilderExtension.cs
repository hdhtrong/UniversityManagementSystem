using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduService.API.Security.Requirements;

namespace EduService.API.Security
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
