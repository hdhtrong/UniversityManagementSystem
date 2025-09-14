using Microsoft.AspNetCore.Authorization;
using ORDService.API.Security.Requirements;

namespace ORDService.API.Security
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
