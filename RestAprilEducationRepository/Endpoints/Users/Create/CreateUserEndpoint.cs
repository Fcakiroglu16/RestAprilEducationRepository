using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Users.Create;

namespace RestAprilEducationRepository.API.Endpoints.Users.Create
{
    public static class CreateUserEndpoint
    {
        public static RouteGroupBuilder AddCreateUserEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/",
                async ([FromBody] CreateUserRequest request,
                       [FromServices] UserApplication userApplication) =>
                    (await userApplication.CreateUserAsync(request)).ToResult());

            return group;
        }
    }
}
