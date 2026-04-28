using RestAprilEducationRepository.API.Endpoints.Users.Create;
using RestAprilEducationRepository.API.Endpoints.Users.Login;

namespace RestAprilEducationRepository.API.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            app.MapGroup("api/users")
                .AddCreateUserEndpoint()
                .AddLoginUserEndpoint();
        }
    }
}
