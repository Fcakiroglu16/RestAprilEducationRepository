

using Asp.Versioning.Builder;

namespace RestAprilEducationRepository.API.Endpoints.Versioning
{
    public static class VersionExampleEndpoints
    {
        public static void AddVersionExampleEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var versionGroupEndpoint = app.MapGroup("api/versions").WithApiVersionSet(apiVersionSet);

            versionGroupEndpoint.MapGet("/", () => Results.Ok("v1.0")).MapToApiVersion(1, 0);
            versionGroupEndpoint.MapGet("/", () => Results.Ok("v2.0")).MapToApiVersion(2, 0);
            versionGroupEndpoint.MapGet("/", () => Results.Ok("v2.1")).MapToApiVersion(2, 1);
        }
    }
}
