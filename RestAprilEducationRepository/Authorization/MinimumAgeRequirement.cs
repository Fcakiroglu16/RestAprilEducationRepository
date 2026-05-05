using Microsoft.AspNetCore.Authorization;

namespace RestAprilEducationRepository.API.Authorization;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
