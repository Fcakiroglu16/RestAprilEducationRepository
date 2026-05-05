using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RestAprilEducationRepository.API.Authorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var birthDateClaim = context.User.FindFirst(ClaimTypes.DateOfBirth);

        if (birthDateClaim is null || !DateTime.TryParse(birthDateClaim.Value, out var birthDate))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        if (age >= requirement.MinimumAge)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
