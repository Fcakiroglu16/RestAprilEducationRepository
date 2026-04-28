using RestAprilEducationRepository.API.Endpoints.Users.Create;

namespace RestAprilEducationRepository.API.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            app.MapGroup("api/users")
                .AddCreateUserEndpoint();
        }
    }
}
