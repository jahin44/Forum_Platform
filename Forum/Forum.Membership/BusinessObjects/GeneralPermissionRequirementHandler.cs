using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Membership.BusinessObjects
{
    public class GeneralPermissionRequirementHandler :
          AuthorizationHandler<GeneralPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                GeneralPermissionRequirement requirement)
        {
            var claims = context.User.Claims;

            foreach (var claim in claims.ToList())
            {
                if (claim.Value == "Moderator" || claim.Value == "User")
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

