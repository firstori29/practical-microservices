using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await userManager.GetUserAsync(context.Subject);

        if (user == null) throw new ArgumentNullException();

        var existingClaims = await userManager.GetClaimsAsync(user);

        if (user.UserName == null) throw new ArgumentNullException();

        var claims = new List<Claim>
        {
            new("username", user.UserName)
        };

        context.IssuedClaims.AddRange(claims);

        context.IssuedClaims.Add(existingClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name) ??
                                 throw new InvalidOperationException());
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}