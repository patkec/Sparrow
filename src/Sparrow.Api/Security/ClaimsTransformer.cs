﻿using System.Linq;
using System.Security.Claims;

namespace Sparrow.Api.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
                return incomingPrincipal;
            
            incomingPrincipal.Identities.First().AddClaim(
                new Claim("localclaim", "localvalue"));

            return incomingPrincipal;
        }
    }
}