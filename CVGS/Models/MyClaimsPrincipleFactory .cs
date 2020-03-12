
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class MyClaimsPrincipleFactory : IUserClaimsPrincipalFactory<User>
    {
        public Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return Task.Factory.StartNew(() =>
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                var principle = new ClaimsPrincipal(identity);

                return principle;
            });
        }

    }

}
