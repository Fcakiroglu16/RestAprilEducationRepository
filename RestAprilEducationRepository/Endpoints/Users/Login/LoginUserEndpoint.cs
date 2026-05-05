using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Users.Login;

namespace RestAprilEducationRepository.API.Endpoints.Users.Login
{
    public static class LoginUserEndpoint
    {
        public static RouteGroupBuilder AddLoginUserEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/login",
                async ([FromBody] LoginRequest request,
                       [FromServices] UserApplication userApplication) =>
                    (await userApplication.LoginAsync(request)).ToResult());

            return group;
        }
    }
}
