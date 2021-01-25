using Microsoft.AspNetCore.Authentication;
using ArchivesFileManagement_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Data
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var ci = (ClaimsIdentity)principal.Identity;
            var user = UserAuth.GetUserRole(ci.Name);
            var role = new Claim(ci.RoleClaimType, user.Role.RoleName);
            ci.AddClaim(role);

            return Task.FromResult(principal);
        }

    }
}
