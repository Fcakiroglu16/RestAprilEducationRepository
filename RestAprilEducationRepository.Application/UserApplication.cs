using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestAprilEducationRepository.Application.Users.Create;
using RestAprilEducationRepository.Application.Users.Login;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application
{
    public class UserApplication(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IConfiguration configuration)
    {
        public async Task<ApplicationResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.UserName,
                BirthDate = request.BirthDate,
            };
            var result = await userManager.CreateAsync(user, request.Password);


            // claim-based (Policy based) authorization için kullanıcıya doğum tarihi bilgisini claim olarak ekleyelim

            await userManager.AddClaimAsync(user,
                new Claim(JwtRegisteredClaimNames.Birthdate, request.BirthDate.ToString(CultureInfo.InvariantCulture)));

            //Role-based authorization için kullanıcıya "editor" rolünü ekleyelim
            await roleManager.CreateAsync(new AppRole() { Name = "editor" });
            await userManager.AddToRoleAsync(user, "editor");


            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApplicationResult<CreateUserResponse>.Failure(errors, HttpStatusCode.BadRequest);
            }

            return ApplicationResult<CreateUserResponse>.Success(
                new CreateUserResponse(user.Id),
                HttpStatusCode.Created);
        }

        public async Task<ApplicationResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ApplicationResult<LoginResponse>.Failure("Email veya şifre hatalı.",
                    HttpStatusCode.Unauthorized);

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return ApplicationResult<LoginResponse>.Failure("Email veya şifre hatalı.",
                    HttpStatusCode.Unauthorized);

            var token = await GenerateJwtToken(user);
            return ApplicationResult<LoginResponse>.Success(token);
        }

        private async Task<LoginResponse> GenerateJwtToken(AppUser user)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var secretKey = jwtSection["SecretKey"]!;
            var issuer = jwtSection["Issuer"]!;
            var audience = jwtSection["Audience"]!;
            var expirationInMinutes = int.Parse(jwtSection["ExpirationInMinutes"]!);


            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new(ClaimTypes.Name, user.UserName.ToString())
            };


            if (!string.IsNullOrEmpty(user.City))
            {
                claims.Add(new("city", user.City!));
            }

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var userClaims = await userManager.GetClaimsAsync(user);


            foreach (var userClaim in userClaims)
            {
                claims.Add(userClaim);
            }


            var expiration = DateTime.UtcNow.AddMinutes(expirationInMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }
    }
}

