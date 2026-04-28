using System.Net;
using Microsoft.AspNetCore.Identity;
using RestAprilEducationRepository.Application.Users.Create;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application
{
    public class UserApplication(UserManager<AppUser> userManager)
    {
        public async Task<ApplicationResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email
            };
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApplicationResult<CreateUserResponse>.Failure(errors, HttpStatusCode.BadRequest);
            }

            return ApplicationResult<CreateUserResponse>.Success(
                new CreateUserResponse(user.Id),
                HttpStatusCode.Created);
        }
    }
}
