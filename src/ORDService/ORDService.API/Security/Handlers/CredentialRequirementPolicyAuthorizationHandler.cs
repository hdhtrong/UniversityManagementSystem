using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ORDService.API.Security.Requirements;
using Shared.SharedKernel;

namespace ORDService.API.Security.Handlers
{
    public class CredentialRequirementPolicyAuthorizationHandler : AuthorizationHandler<CredentialRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CredentialRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var hasClaim = context.User.Claims.Any(x => x.Type == Constants.CREDENTIAL_CLAIM && x.Value == requirement.ClaimValue);

            if (hasClaim)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
