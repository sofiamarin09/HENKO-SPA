using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using HankoSpa.Models;

namespace HankoSpa.Helpers
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Solo agrega el claim si el CustomRolId es válido
            if (user.CustomRolId > 0)
            {
                identity.AddClaim(new Claim("CustomRolId", user.CustomRolId.ToString()));
            }

            return identity;
        }
    }
}

